using Swashbuckle.AspNetCore.Annotations;

namespace MinimalAPIDemo;


public static class ProductsEndPoint
{
    
    public static void ConfigurepProductApi(this WebApplication app)
    {
        app.MapGet("/products/{id}", (int id) => { return Results.Ok(); }).WithMetadata(new SwaggerOperationAttribute("Get Product BY ID", "More Description of the method")); ;
    }


}


