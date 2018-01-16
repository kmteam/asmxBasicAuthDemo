# Basic auth on ASP.NET web service (asmx)

A basic example about how to implement [http basic access authentification](https://en.wikipedia.org/wiki/Basic_access_authentication) on asp.net classic webservice.
[See commits](https://github.com/kmteam/asmxBasicAuthDemo/commits/master) for a step by step guide.

### Prior notice
- SSL must be configured in order to be **'secure'**.
- Microsoft considers asmx as **legacy technology**. XML Web services and XML Web service clients should now be created using [Windows Communication Foundation (WCF)](https://docs.microsoft.com/en-us/dotnet/framework/wcf/index) but fortunately, **this already works with WCF**!

### Steps on the service.
 1. [Create your own authentification module by implementing IHttpModule](https://github.com/kmteam/asmxBasicAuthDemo/commit/6bd7aacac24df3d5bc4f1f8118575d8c7b8afcb5)
 2. [Add auth module to app.config file](https://github.com/kmteam/asmxBasicAuthDemo/commit/1dc7c0803859c8517dbac50ad9612f153990b149)
 3. That's it!
 
### Example of client code

```
var client = new ServiceReference1.ServiceSoapClient();
client.ClientCredentials.UserName.UserName = "root";
client.ClientCredentials.UserName.Password = "secret12345";
try
{
  var res = client.Add(3, 4);
  Console.WriteLine(res);
}
catch (Exception ex)
{
  Console.WriteLine(ex.Message);
}
```

You must edit your client app.config file and add.
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <system.serviceModel>
    <bindings>
      <basicHttpBinding>
        <binding name="ServiceSoap">
          <security mode="TransportCredentialOnly"> <!-- Set Http basic auth!-->
            <transport clientCredentialType="Basic" />
          </security>
          </binding>
      </basicHttpBinding>
    </bindings>
    <client>
      <endpoint address="http://localhost:5838/Service.asmx" binding="basicHttpBinding"
      bindingConfiguration="ServiceSoap" contract="ServiceReference1.ServiceSoap"
      name="ServiceSoap" />
    </client>
  </system.serviceModel>
</configuration>
```
