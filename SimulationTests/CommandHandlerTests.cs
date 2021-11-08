using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABB.InSecTT.Common;
using System.Threading.Tasks;
using System;
using System.IO;

namespace SimulationTests
{
    [TestClass]
    public class CommandHandlerTests
    {
        [TestMethod]
        public void CommandTest1()
        {
            int val = 0;
            ICmd command = new Command("t", "bb", (s) => val = 100);
            Assert.AreEqual(command.CommandDescription, "bb");
            Assert.AreEqual(command.Usage, "t");
            command.Execute(string.Empty);
            Assert.AreEqual(val, 100);
        }


        [TestMethod]
        public void CommandTest2()
        {        
            using (var writer = new StringWriter())
            {
                using (var reader = new StringReader("x" + writer.NewLine))
                {
                    Console.SetOut(writer);
                    Console.SetIn(reader);
                    MenuHandler mh = new MenuHandler(new ICmd[] { });
                    Task.Run(() => mh.HandleCommand()).Wait();
                    writer.Flush();
                }
                var consoleData = writer.GetStringBuilder().ToString();
                Assert.IsTrue(consoleData.Contains("x = "));
            }

        }

        [TestMethod]
        public void CommandTest3()
        {            
            int val = 0;            
            ICmd command = new Command("t", "bb", (s) =>
            {
                val = 100;
            });

            using (var writer = new StringWriter())
            {
                using (var reader = new StringReader("t" + writer.NewLine + "x" + writer.NewLine))
                {
                    Console.SetOut(writer);
                    Console.SetIn(reader);
                    MenuHandler mh = new MenuHandler(new ICmd[] { command }); 
                    Task.Run(() => mh.HandleCommand()).Wait();
                    writer.Flush();
                }
                var consoleData = writer.GetStringBuilder().ToString();
                Assert.AreEqual(val, 100);
                Assert.IsTrue(consoleData.Contains("bb" + writer.NewLine));
            }
        }
    }
}
