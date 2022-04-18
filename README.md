# Cryptocurrency Bot
## About
This is the second revision for this project as the first revision works, but because of the project structure it's unable to be truly expansable.

## Aim
The aim of this project is to view the current asking/bidding/sale prices of each trade pair and their respective assets, monitor the price change of each asset and make an informed decision to either buy, sell or hold the asset based on its current performance.

The application will be using the services of Binance and Coinbase and maybe more in the future.
The reason for this (without delving too much into my trading strategies) is to get any asset (that is listed on both marketplaces), bid the lowest price to buy the asset, transfer it to a cold wallet and monitor its performance on both marketplaces. Conversely I will monitor the status of any crypto assets that I hold in my cold wallet and monitor the asking price for each asset and place the asset for sale when I've reached my threshold for profit.

The project will eventaully be placed in a Virutal Machine to let the process run 24/7, the decided service to use to allow me to do this is still in its planning stages, although I'm looking to use Docker to do this.

Once the Minimum Viable Product has been made, I will host this project and because of the structure, I will be able to add features and microservices to enhance the user experience.

At some stage this project will no longer be Open Source and I will start to charge users user for each successful trade.

## Features to look forward to

I'll be using Xamarin (Soon to be Mono) to develop a (pseudo) platform agnostic User Interface during November to allow the user to visually see what the program is doing instead of using a console application on a windows machine/vm.

## Code
Alright, the bit where most of you are waiting for, although the code itself has more comments than I can put on here, so please check it out.

# **HttpService**
The core of the application. HttpService is an abstract class that implements **IHttpService** and its implementations. The reason why this is structured this way is because since I've worked with both Binance and Coinbase's API Service there are core componenets that they both use for example (taken from the class **'BinanceService'** which has the implementation of a method from IHttpService):
```
    /// <summary>
    /// Helper method to send an asynchoronus call to binance's endpoints.
    /// </summary>
    /// <param name="httpMethod">'GET', 'POST', 'DELETE'.</param>
    /// <param name="requestUri">The address of the endpoint.</param>
    /// <param name="content">The content to go with the call.</param>
    /// <returns>A response string in JSON.</returns>
    public override async Task<string> SendAsync(HttpMethod httpMethod, string requestUri, object? content = null)
    {
      using (var request = new HttpRequestMessage(httpMethod, BaseUri + requestUri))
      {
        request.Headers.Add("X-MBX-APIKEY", ApiKey);

        if (!(content is null))
          request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await Client.SendAsync(request);

        using (HttpContent responseContent = response.Content)
        {
          string jsonString = await responseContent.ReadAsStringAsync();

          return jsonString;
        }
      }
    }
```
This method is used by other methods from IHttpService such as SendPublicAsync & SendSignedAsync (decomposition of the code allowed for better flexibility) to make their calls respectively as someendpoints require an authenticated string which uses the client's api secret and api key.