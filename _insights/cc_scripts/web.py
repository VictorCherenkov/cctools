from System.Net import WebClient
from System.Net import CookieContainer

class WebClientEx (WebClient):
    @property
    def Cookies(self):
        return self._Cookies
    
    @Cookies.setter
    def Cookies(self, value):
        self._Cookies = value
    
    def GetWebRequest(self, address):
        request = WebClient.GetWebRequest(self, address)
        if request is not None and self._Cookies is not None:
            request.CookieContainer = self._Cookies
        return request
        
def download_file(url, file_name):
    with WebClientEx() as client:
        client.UseDefaultCredentials = True
        client.Cookies = CookieContainer()
        client.DownloadFile(url, file_name)    

def download(url):
    with WebClientEx() as client:
        client.UseDefaultCredentials = True
        client.Cookies = CookieContainer()
        return client.DownloadString(url)    
        
    
