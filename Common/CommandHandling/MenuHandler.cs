using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

namespace ABB.InSecTT.Common
{

    public class MenuHandler
    {
        List<ICmd> m_commands;
        private bool m_loop = true;

        public MenuHandler(IEnumerable<ICmd> commands)
        {
            m_commands = new List<ICmd>(commands);
            m_commands.AddRange(CreateDefaultCommands());


        }

        public async Task<long> HandleCommand()
        {
            PrintHelp();

            while (m_loop == true)
            {

                string cmd = await Console.In.ReadLineAsync();
                var cmdSplit = cmd.Split(' ');
                


                foreach (ICmd command in m_commands)
                {
                    if (string.IsNullOrWhiteSpace(cmdSplit[0]))
                    {
                        continue;
                    }
                    else if (command.Usage == cmdSplit[0].ToLower())
                    {
                        command.Execute(cmdSplit.Length == 1 ? String.Empty : cmdSplit[1]);
                    }

                }

            }

            return 0;
        }       

        private IEnumerable<ICmd> CreateDefaultCommands()
        {
            Action<string> printHelp;
            Action<string> exitProgram;
            printHelp = (s) => PrintHelp();
            exitProgram = (s) => Exit();
            yield return new Command("h", "h = prints this help", printHelp);
            yield return new Command("x", "x = quits the program", exitProgram);
        }

        private void PrintHelp()
        {
            foreach (ICmd command in m_commands)
            {
                Console.WriteLine(command.CommandDescription);

            }

        }
        private void Exit()
        {
            m_loop = false;
        }
    }

}