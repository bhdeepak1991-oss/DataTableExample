using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DataTableExample
{
    public static class ViewRenderHelper
    {
        public static async Task<string> RenderViewAsync<TModel>(
            this Controller controller,
            string viewPath,
            TModel model,
            bool isPartial = false)
        {
            controller.ViewData.Model = model;

            using var writer = new StringWriter();

            var viewEngine = controller.HttpContext.RequestServices
                .GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

            var viewResult = viewEngine.GetView(null, viewPath, !isPartial);

            if (!viewResult.Success)
                throw new FileNotFoundException($"View {viewPath} not found");

            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }
}
