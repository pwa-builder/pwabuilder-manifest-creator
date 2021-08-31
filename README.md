# pwabuilder-manifest-creator
Scrapes a web page and creates a web manifest from metadata within the page

## Running the service
The service is published as an Azure function at https://pwabuilder-manifest-creator.azurewebsites.net. You can run the service by issuing a GET to `https://pwabuilder-manifest-creator.azurewebsites.net/api/create?url=https://sample.com`, where sample.com is the website for which you wish to generate a manifest.

## Running locally
To run the function locally, clone the repository, open `Microsoft.PWABuilder.ManifestCreator.sln` in Visual Studio, and press F5 to run. Visual Studio will prompt you to install any missing Azure tools needed to run the solution.
