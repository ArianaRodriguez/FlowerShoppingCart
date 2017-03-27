# Flower Shopping Cart
Flower Xochimilico is a simulation of a shopping cart:

    * Visual Studio Solution
    * Paypal REST API
    * Payment with Paypal method
    * Unit Test

## Prerequisites
* Paypal account API credentials to be used in the web application.
* Buyer account. (You can create these accounts by registering on Paypal Developer website here https://developer.paypal.com for Free.)
* Paypal SDK for .NET NuGet Package Manager.

## Installation

* Clone 

```shell
git clone git@github.com:ArianaRodriguez/FlowerShoppingCart.git
```

* Install the package on Visual Studio

	Tools -> Library Package Manager -> Package Manager Console

```shell
	Install-Package Paypal
```

* Setup the web.config file with your configuration.

```config
<configSections>
  <section name="paypal" type="PayPal.SDKConfigHandler, PayPal" />
</configSections>

<!-- PayPal SDK settings -->
<paypal>
  <settings>
    <add name="mode" value="sandbox"/>
    <add name="connectionTimeout" value="360000"/>
    <add name="requestRetries" value="1"/>
    <add name="clientId" value="your client ID of paypal account"/>
    <add name="clientSecret" value="your client secret key of paypal account"/>
  </settings>
</paypal
```

* Run the project

* Run Tests

	Test -> Run -> All Tests
	
## Explore the app

Visit [`http://flowershippingcart.azurewebsites.net/`](http://flowershippingcart.azurewebsites.net/)
Click on "Buy" link to simulate PayPal Paypment
 

## Links of interes

* [`http://developer.paypal.com`]
* [`https://developer.paypal.com/docs/integration/direct/express-checkout/integration-jsv4/advanced-payments-api/`]


## Author

- Written by [Ariana Rodriguez]

