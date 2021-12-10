# LocalFSService

REST API Service to access local file system from cloud

# Usage

After cloning, you need to:

1. enter your settings into the default settings of the service.
   1. RootPath - path, to which is the service limited and will be used as root when relative path is passed to the API
   2. User - user name for the basic auth
   3. Pwd - password for the basic auth
   4. RelayNameSpace - name of the namespace of Azure Relay service to be used
   5. RelayServicePrefix - prefix to be used when registering into Azure Relay
2. set SAS key for your Azure Relay namespace in app.config (look for {enter your key})


Service will be available on this URL:
http://Localhost:8787
https://{RelayNameSpace}.servicebus.windows.net/{RelayServicePrefix}/{computername}.{domainname}

Authorization: basic, use the User and Pwd you set in the config file
