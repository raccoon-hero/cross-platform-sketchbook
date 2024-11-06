# ASP.NET Core MVC Web Application with Auth0

This is a web application built using ASP.NET Core MVC and integrated with Auth0 for authentication. It consists of three main parts: an MVC web application (`Application`), a class library that solves all tasks from previous works (`KPCore5`), a vagrant environment (`Vagrant`), and a data samples folder for each implemented task solution (`DataSamples`).

The instructions below will get you a copy of the project up and running on your local machine for development and testing purposes. They also demonstrate how to set up user registration, login, and profile pages, as well as how to handle user claims and authorization.

*Web App's Main Page:*
![Web App Main Page](https://github.com/raccoon-hero/cross-platform-sketchbook/blob/main/Images/CROSS_Task5.%20Screenshot%201.%20Main%20Page.png)

## Prerequisites

![.NET SDK](https://img.shields.io/badge/.NET%20SDK-Compatible-blue)
![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)
![Vagrant](https://img.shields.io/badge/Vagrant-VM%20Manager-blue)

Before you begin, ensure you have the following installed:
- [**.NET SDK**](https://dotnet.microsoft.com/download), compatible with the version used in the project (`.NET 6`).
- [**Visual Studio 2022**](https://visualstudio.microsoft.com/vs/) or a similar IDE that supports .NET development.
- [**Auth0**](https://auth0.com/) account and Auth0 Application credentials.
- [**Vagrant**](https://www.vagrantup.com/downloads.html): Used for setting up and managing virtual machines for deployment testing. 

## Set up

   1. Clone the repository:

   ```bash
   git clone https://github.com/raccoon-hero/cross-platform-sketchbook.git
   ```

   2. Navigate to the project directory:

   ```bash
   cd cross-platform-sketchbook/CROSS_Task5/Application
   ```

   3. Create your own / Modify the existing `appsettings.json` file in the project root and add your Auth0 credentials (`ClientSecret` isn't obligatory to fill in for this project):

   ```json
   "Auth0": {
      "Domain": "your-auth0-domain",
      "ClientId": "your-auth0-client-id",
      "ClientSecret": "your-auth0-client-secret"
   }
   ```
   4. Build and run the application:

   ```bash
   dotnet build
   dotnet run
   ```

   The application will be accessible at `http://localhost:3000`.
## Additional Sign-up Data Handling with Auth0 Universal Login & Actions

   The sign-up page beside Password & Email should also have Username, Full Name, and Phone Number parameters. Default Auth0 doesn't have direct ways to do this. But, we can set up a Universal Login Page with Auth0, where we have a bigger control over what kind of information we want to ask our users.

   1. **Universal Login:**

      To customize the Universal Login page in Auth0 for the web application, go to the `Customize Login Page` panel, located here: `Branding → Universal Login → Advanced Options → Login → Customize Login Page`. In this tab, you should switch on the feature and you will get a possibility below to modify the HTML layout of your login page.
   
      For this project, the following HTML layout was configured (partly using `Lock` template by Auth0):
      ```html
      <!DOCTYPE html>
      <html>
      <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
        <title>Sign In with Auth0</title>
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0" />
      </head>
      <body>
        <script src="https://cdn.auth0.com/js/lock/12.3/lock.min.js"></script>
        <script>
          var config = JSON.parse(decodeURIComponent(escape(window.atob('@@config@@'))));
          config.extraParams = config.extraParams || {};
          var connection = config.connection;
          var prompt = config.prompt;
          var languageDictionary;
          var language;
      
          if (config.dict && config.dict.signin && config.dict.signin.title) {
            languageDictionary = { title: config.dict.signin.title };
          } else if (typeof config.dict === 'string') {
            language = config.dict;
          }
          var loginHint = config.extraParams.login_hint;
          var colors = config.colors || {};
      
          // Define additional signup fields
          var additionalSignUpFields = [
            {
              name: "full_name",
              placeholder: "Enter your full name",
              validator: function(value) {
                return {
                  valid: value.length >= 1 && value.length <= 500,
                  hint: "Must have 1-500 characters" // optional
                };
              }
            },
            {
              name: "phone_number",
              placeholder: "Enter your phone number",
              validator: function(value) {
                var phonePattern = /^(\+380)[0-9]{9}$/; // Adjust the regex for Ukrainian phone format
                return {
                  valid: phonePattern.test(value),
                  hint: "Must be a valid Ukrainian phone number" // optional
                };
              }
            }
          ];
      
          var lock = new Auth0Lock(config.clientID, config.auth0Domain, {
            auth: {
              redirectUrl: config.callbackURL,
              responseType: (config.internalOptions || {}).response_type ||
                (config.callbackOnLocationHash ? 'token' : 'code'),
              params: config.internalOptions
            },
            configurationBaseUrl: config.clientConfigurationBaseUrl,
            overrides: {
              __tenant: config.auth0Tenant,
              __token_issuer: config.authorizationServer.issuer
            },
            assetsUrl:  config.assetsUrl,
            allowedConnections: connection ? [connection] : null,
            rememberLastLogin: !prompt,
            language: language,
            languageBaseUrl: config.languageBaseUrl,
            languageDictionary: languageDictionary,
            theme: {
              primaryColor: colors.primary ? colors.primary : 'green'
            },
            closable: false,
            defaultADUsernameFromEmailPrefix: false,
            additionalSignUpFields: additionalSignUpFields
          });
      
          if(colors.page_background) {
            var css = '.auth0-lock.auth0-lock .auth0-lock-overlay { background: ' +
                        colors.page_background +
                      ' }';
            var style = document.createElement('style');
            style.appendChild(document.createTextNode(css));
            document.body.appendChild(style);
          }
      
          lock.show();
        </script>
      </body>
      </html>
      ```
   2. **Actions:**

      After setting up the Universal Login page, there's also a need to configure Auth0 Login/Post-Login Actions. Go to Auth0 Dashboard, and navigate to the `Actions` section. There сreate a New Action, choose `Custom` under the `Login / Post Login`.
   
      Here's a script used within this project:
      ```js
      exports.onExecutePostLogin = async (event, api) => {
        if (event.user.user_metadata && event.user.user_metadata.phone_number) {
          const namespace = 'https://claims.example.com/';
          api.idToken.setCustomClaim(namespace + 'phone_number', event.user.user_metadata.phone_number);
          api.idToken.setCustomClaim(namespace + 'full_name', event.user.user_metadata.full_name);
          api.idToken.setCustomClaim(namespace + 'username', event.user.username);
        }
      };
      ```
   
      Deploy Action after setting everything up, and add it to the flow. You can replace `https://claims.example.com/` with any link of your choice.

   3. **Handlers:**

      Look at `Program.cs` & `AccountController.cs` in the `Application` folder, to see how the handling was implemented.

## Vagrant
   1. **Initialize Vagrant in the Project:**

      Navigate to the `Vagrant` directory and run the following command:
      ```bash
      vagrant init debian/bookworm64
      ```
      You can find more Vagrant boxes on [Vagrant Cloud](https://app.vagrantup.com/boxes/search).
   
   2. **Configure the Vagrantfile:**
      
      `init` generates a `Vagrantfile` of the VW. Edit it to set up the VM for your needs. The one used within this project is placed in the `Vagrant` folder. Here's an example of Vagrantfile configured for this project (``Debian`` set up with downloading `.NET6`, `git`):

      ```bash
      Vagrant.configure("2") do |config|
         config.vm.box = "debian/bookworm64"
         config.vm.network "public_network"
         config.vm.network "forwarded_port", guest: 3000, host: 3000

         config.vm.provision "shell", inline: <<-SHELL
            apt-get update
            apt-get upgrade

            wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
            sudo dpkg -i packages-microsoft-prod.deb
            rm packages-microsoft-prod.deb

            sudo apt-get update && sudo apt-get install -y dotnet-sdk-6.0
            sudo apt-get install -y git
            
            git clone https://github.com/raccoon-hero/cross-platform-sketchbook.git
            cd cross-platform-sketchbook/CROSS_Task5/Application
            dotnet run
         SHELL
      end

   4. **Start & Access the VM:**
      ```bash
      vagrant up
      vagrant ssh
      ```

   5. **Additional Commands to Consider:**
      
      **[Check Permission]:** To double-check if the Vagrant user has permission to access all needed folders, including `CROSS_Task5.sln`, use this command:
      
      ```bash
      ls -l [path]
      ls -l /home/vagrant/cross-platform-sketchbook/CROSS_Task5/Application
      ```

      **[Give Permission]:** In case the Vagrant user doesn't have all required permissions to access project folders or specific files, use this command:

      ```bash
      sudo chown -R vagrant:vagrant [path]
      sudo chown -R vagrant:vagrant ~/cross-platform-sketchbook
      ```

## Usage

Core features of this Web App Project:
- Access the application at `http://localhost:3000`.
- Register a new user with a unique username, full name, password, phone number (in Ukraine format), and RFC 822 email.
- Log in with your registered credentials.
- Explore the Profile page, which displays the user's information.
- Explore the Home page, which displays all Tasks with corresponding interactive pages
- Log out from the application (you will loose access to interactive pages).

## Additional Images

*Web App's Task 1 Page:*
![Web App Task Page Example](https://github.com/raccoon-hero/cross-platform-sketchbook/blob/main/Images/CROSS_Task5.%20Screenshot%202.%20Task%20Page%20Example.png)
