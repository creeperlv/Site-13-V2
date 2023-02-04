using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Site13Kernel.UI.xUI;
using System;
using System.Diagnostics;
using xUI.Core;
using xUI.Core.Abstraction;

namespace Site13Kernel.Tests
{
    [TestClass]
    public class xUITest
    {
        [TestMethod]
        public void composer_test()
        {
            string UIContent =
                @"<Window Title=""Sample"">
<Grid>
    <Text>Sample Text</Text>
    <Text>Sample Text2</Text>
    <Grid>
      <Text>Sample Text3</Text>
      <Text>Sample Text4</Text>
    </Grid>
</Grid>
</Window>";
            UIComposer.Init();
            var a = UIComposer.Parse(UIContent);
            //if(fal)
            //ShowUIElement((s,i) =>
            //{
            //    for (int _i = 0; _i < i; _i++)
            //    {
            //        Trace.Write("\t");
            //    }
            //    Trace.WriteLine("Result:" + s);
            //}, a,0);
            Trace.WriteLine(JsonConvert.SerializeObject(a, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            }));
        }
        void ShowUIElement(Action<string, int> print, IUIElement element, int Depth)
        {
            print("" + element.GetType().Name, Depth);
            if (element is IxUIContainer)
            {
                if (element.Children != null)
                {
                    foreach (var item in element.Children)
                    {
                        //print("iContainer Child:" + item.GetType().Name);
                        ShowUIElement(print, item, Depth + 1);
                    }
                }
            }
            if (element is IContent c)
            {
                if (c.Content != null)
                    if (c.Content is IUIElement e)
                    {
                        //print("Content:"+e.GetType().Name);
                        ShowUIElement(print, e, Depth + 1);

                    }
                    else if (c.Content is String)
                    {
                        print("String Content:" + (String)c.Content, Depth + 1);
                    }
            }
        }
    }
}
