from argparse import ArgumentParser
import web
import os
import fileinput
import json
import subprocess
import sys

def get_spec_url(product, release, branch, make_sr_branch, use_nightly_build, ignore_branch_type_verification, output_format):
    service_url = "http://css.mis.amat.com/www1/cmweb/services/cmweb.srvc.mconfig-spec.php"    
    spec_url = "{0}?ServiceCMD=QuickGet&productId={1}&release={2}&sr={3}&makeSR={4}&useNB={5}&ignoreBTV={6}&outputFormat={7}".format(
        service_url, product, release, branch, make_sr_branch, use_nightly_build, ignore_branch_type_verification, output_format)
    return spec_url

def get_sr_info_url(product, branch):
    service_url = "http://css/www/cmweb/services/cmweb.srvc.review.php"    
    sr_info_url = "{0}?ServiceCMD=GetSRInfo&productId={1}&sr={2}&isSR=true".format(
        service_url, product, branch)
    return sr_info_url
        
def parse_args():
    parser = ArgumentParser()
    parser.add_argument("-p", "--product", required=True, help="ID of your product/component (e.g. CGAFE).")
    parser.add_argument("-r", "--release", default="LATEST", help="Release number (e.g. 2.21). You can also use LATEST keyword to specify latest release [default: %(default)s].")
    parser.add_argument("-b", "--branch", required=True, help="SR number or branch (e.g. cgafesr1000).")
    parser.add_argument("-m", "--make_sr_branch", action="store_true", default=False, help="Whether to make SR branch [default: %(default)s].")
    parser.add_argument("-n", "--use_nightly_build", action="store_true", default=False, help="Whether to use Nightly build [default: %(default)s].")
    parser.add_argument("-i", "--ignore_branch_type_verification", action="store_true", default=True, help="Whether to ignore branch type verification [default: %(default)s]")
    parser.add_argument("-f", "--output_format", choices=["text", "xml"], default="text", help="Desirable output format. Use \"text\" value for the plain text format. Use \"xml\" value for the xml format. [default: %(default)s]")
    args = parser.parse_args()
    return args

def bool_to_string(value):
    return "true" if value else "false"

def set_description(description):
    cwd = os.getcwd()                      
    root, dname = os.path.split(cwd)
    pattern = dname + ' '
    filepath = os.path.join(root, "descript.ion")
    updated = False
    if os.path.isfile(filepath):
        for line in fileinput.input(filepath, inplace=1):
            line = line.rstrip("\r\n")
            if len(line) > 0:
                if not line.startswith(pattern):
                    print(line)
                else:
                    print(dname + ' ' + description)
                    updated = True
    if not updated:
        with open(filepath, "a") as description_file:
            description_file.write(dname + ' ' + description)    

def main():
    args = parse_args()    
    # Do not allow web service to create SR branch type regardless of args.make_sr_branch value,
    # as web service sets branch type owner to Clearcase_Web
    spec_url = get_spec_url(args.product, args.release, args.branch, bool_to_string(False), bool_to_string(args.use_nightly_build), bool_to_string(args.ignore_branch_type_verification), args.output_format)    
    spec_file_name = ".spec"        
    description = "{0}|{1} {2}".format(args.branch, args.product, args.release)
    print("Obtaining config spec for %s" % description)
    web.download_file(spec_url, spec_file_name)
    print("Setting config spec")    
    if not subprocess.call(["cleartool", "setcs", "-force", spec_file_name]) == 0:
        sys.exit(-1)
    print("Obtaining SR info for %s" % description)
    sr_info_url = get_sr_info_url(args.product, args.branch)
    sr_info_json = web.download(sr_info_url)    
    sr_info = json.loads(sr_info_json)
    if sr_info["responseOK"]:
        # Create SR branch type in all associated VOBs except CGAShared, if requested        
        if args.make_sr_branch:
            for vob in sr_info["VOBs"]:
                if not vob == "CGAShared":
                    print("Creating branch type {0}@\\{1}".format(args.branch, vob))                                        
                    subprocess.call(["cleartool", "mkbrtype", "-nc", "{0}@\\{1}".format(args.branch, vob)])
        print(sr_info["Title"])
        set_description("{0}|{1}".format(description, sr_info["Title"]))
    else:
        print("Failed to get SR info: {0}".format(sr_info["serviceMessage"]))
        if args.make_sr_branch:
            print("Branch type {0} is NOT created: Unable to determine associated VOBs".format(args.branch))
        set_description(description)

if __name__ == "__main__":
    main()