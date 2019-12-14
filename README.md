> Projet API .NET Core 2.2 utilisant Swagger

# Documentation d'API avec Swagger

## Création du projet

* Dans le menu **Fichier**, sélectionnez **Nouveau** > **Projet**.
* Sélectionnez le modèle **Application web ASP.NET Core** et cliquez sur **Suivant**.
* Nommez le projet *TodoApi* et cliquez sur **Créer**.
* Dans la boîte de dialogue **créer une application Web ASP.net Core** , vérifiez que **.net Core** et **ASP.net Core 3.1** sont sélectionnés. Sélectionnez le modèle **API** et cliquez sur **Créer**.

## Ajout du package

- Dans le menu **Outils**, sélectionnez **Gestionnaire de package NuGet > Gérer les packages NuGet pour la solution**.
- Sélectionnez l’onglet **Parcourir**, puis entrez **Swashbuckle.AspNetCor**e dans la zone de recherche.
- Dans le volet gauche, sélectionnez **Swashbuckle.AspNetCore**.
- Cochez la case **Projet** dans le volet droit, puis sélectionnez **Installer**.

## Configuration du middleware

* Dans le fichier startup, ajouter le namespace :

```c#
using Swashbuckle.AspNetCore.Swagger;
```

* Ajouter également le service Swagger :

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        .AddJsonOptions(options =>
        {
        options.SerializerSettings.Formatting = Formatting.Indented;
        });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
    });
}
```

* Ajouter le middleware Swagger :

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCookiePolicy();

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });


    app.UseMvc();
}
```

## Utilisation

* Url d'accès à la documentation JSON : https://localhost:44345/swagger/v1/swagger.json
* Url d'accès à swagger UI : https://localhost:44345/swagger/index.html

## Ajouter des commentaires à la documentation

> Les commentaires permettent de compléter la documentation en + de celui générer par défaut.

* Modifier le fichier .csproj du projet, pour activer la prise en charge de la documentation XML :

```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

* Dans le fichier Startup.cs, ajouter les namespace :

```c#
using System.Reflection;
using System.IO;
```

* Modifier également la méthode :

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
         .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
         .AddJsonOptions(options =>
         {
             options.SerializerSettings.Formatting = Formatting.Indented;
         });

    // Register the Swagger generator, defining 1 or more Swagger documents
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Info
        {
            Version = "v1",
            Title = "PrintFramer API",
            Description = "Calculates the cost of a picture frame based on its dimensions.",
            TermsOfService = "None",
            Contact = new Contact
            {
                Name = "Your name",
                Email = string.Empty,
                Url = "https://www.microsoft.com/learn"
            }
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
}
```

* Ajout de commentaire sur les endpoints, exemple :

```c#
 /// <summary>
 /// Returns the price of a frame based on its dimensions.
 /// </summary>
 /// <param name="Height">The height of the frame.</param>
 /// <param name="Width">The width of the frame.</param>
 /// <returns>The price, in dollars, of the picture frame.</returns>
 /// <remarks> The API returns 'not valid' if the total length of frame material needed (the perimeter of the frame) is less than 20 inches and greater than 1000 inches.</remarks>
 [HttpGet("{Height}/{Width}")]
 public string GetPrice(string Height, string Width)
 {
     string result;
     result = CalculatePrice(Double.Parse(Height), Double.Parse(Width));
     return $"The cost of a {Height}x{Width} frame is ${result}";
 }
```

## Prise en charge des annotations DataAnnotaions

* Les annotations issues du namespace **System.ComponentModel.DataAnnotation** sont pris en compte par défaut dans la génération de la documentation.
* Exemple de dataAnnotation :
  * [Produces("text/plain")]
  * [StringLength(10)]
  * [StringLength(10, MinimumLength = 5)]
  * [Required]
  * ...

## Ajouter des annotations spécifiques à Swagger

* Il est possible d'ajouter des annotations spécifique à Swagger afin de compléter les informations de la documentation par défaut.
* Ajouter le package **Swashbuckle.AspNetCore.Annotations**
* Dans le fichier **startup.cs**, activer les annotations dans la méthode **ConfigureServices.cs** :

```c#
services.AddSwaggerGen(c =>
{
   ...
   c.EnableAnnotations();
});
```

* Il est maintenant possible d'utiliser de nouvelles annotations pour enrichir la documentation.

```c#
[HttpGet]
[SwaggerOperation(Summary = "Gets two values", Description = "Gets two hardcoded values")]
[SwaggerResponse(200, "I guess everything worked")]
[SwaggerResponse(400, "BAD REQUEST")]
public IEnumerable<string> Values()
{
    return new string[] { "value1", "value2" };
}
```

