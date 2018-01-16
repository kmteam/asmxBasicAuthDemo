# Basic auth on ASP.NET web service (asmx)

A basic example about how to implement [http basic access authentification](https://en.wikipedia.org/wiki/Basic_access_authentication) on asp.net classic webservice.
[See commits](https://github.com/kmteam/asmxBasicAuthDemo/commits/master) for a step by step guide.

### Prior notice
- SSL must be configured in order to be **'secure'**.
- Microsoft considers asmx as **legacy technology**. XML Web services and XML Web service clients should now be created using [Windows Communication Foundation (WCF)](https://docs.microsoft.com/en-us/dotnet/framework/wcf/index) but fortunately, **this already works with WCF**!


### Example of calling code

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
