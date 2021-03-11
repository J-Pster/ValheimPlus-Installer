using System;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace ValheimBrasil
{
    internal class Program
    {
        public string dirgame = "C:/Program Files (x86)/Steam/steamapps/common/valheim";
        public string dirselected = null;

        //
        enum opcao
        {
            ValheimPlus = 1,
            Desinstalar,
            Atualizar,
        };

        public static void Menu()
        {
            Console.WriteLine("========== Valheim+ Installer ==========\n");
            Console.WriteLine("WARNING: THE GAME MUST BE TURNED OFF!");
            Console.WriteLine("Welcome to the BepInEx + ValheimPlus installer");
            Console.WriteLine("This application is designed to install ValheimPlus, it basically opens a WebClient to download the .zip,\nthen extracts it into the selected folder and deletes the downloaded .zip.\n");
            Console.WriteLine("This application is MIT, the source code is in the repository: ");
        }

        
        public static void InstallMenu()
        {
            Menu();
            Console.WriteLine("What you want to do?");
            Console.WriteLine("[1] Install Valheim+");
            Console.WriteLine("[2] Uninstall Valheim+");
            Console.WriteLine("[3] Update Valheim+");
            Console.Write("> ");
        }

        public bool TestDefaultDirectory()
        {
            Menu();
            Console.WriteLine("Testing default directory...");
            bool exists = System.IO.Directory.Exists(dirgame);

            if (exists)
            {
                Console.WriteLine("Standard Deritory found! Looking for Valheim.exe");
                Directory.SetCurrentDirectory(dirgame);
                System.Threading.Thread.Sleep(2000);
                SearchingValheimExe(dirgame);
                Console.WriteLine("Valheim.exe has been found, continuing...");
                SearchingBepInExInstall(dirgame);
                System.Threading.Thread.Sleep(3000);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Default directory not found, moving on...");
                System.Threading.Thread.Sleep(8000);
                Console.Clear();
            }

            return exists;
        }

        public static void SearchingDirectory(string dir)
        {
            bool exists = System.IO.Directory.Exists(dir);
            try
            {
                Directory.SetCurrentDirectory(dir);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("\nDirectory not found: " + $"'{dir}'");
                System.Threading.Thread.Sleep(2500);
                Console.Clear();
                Program programa = new Program();
                programa.SelectDirectory();
            }

            if (exists)
            {
                Console.WriteLine("\nDirectory Found, looking for valheim.exe");
                SearchingValheimExe(dir);
                System.Threading.Thread.Sleep(2000);
            }
        }

        public static void SearchingValheimExe(string dir)
        {
            bool existsexe = System.IO.File.Exists($"{dir}/valheim.exe");
            if (existsexe)
            {
                Console.WriteLine("Valheim.Exe was found, continuing installation...");
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("Valheim.Exe was not found, restarting...");
                System.Threading.Thread.Sleep(4000);
                Console.Clear();
                Program programa = new Program();
                programa.SelectDirectory();
            }
        }

        public static void SearchingBepInExInstall(string dir)
        {
            Console.WriteLine("SearchingBepInEx Dir Received: " + dir);
            bool existsbepinex = false;
            bool existsdoorstop = System.IO.File.Exists($"{dir}/doorstop_config.ini");
            bool existswinhttp = System.IO.File.Exists($"{dir}/winhttp.dll");
            bool existsbepfolder = System.IO.Directory.Exists($"{dir}/BepInEx");
            bool existsdorstoplibs = System.IO.Directory.Exists($"{dir}/doorstop_libs");
            bool existsunstripped = System.IO.Directory.Exists($"{dir}/unstripped_corlib");
            
            if (existsdoorstop || existsbepfolder || existsdorstoplibs || existswinhttp || existsunstripped)
            {
                Console.WriteLine("existsunstripped" + existsunstripped);
                Console.WriteLine("existswinhttp" + existswinhttp);
                Console.WriteLine("existsbepfolder" + existsbepfolder);
                Console.WriteLine("existsdorstoplibs" + existsdorstoplibs);
                Console.WriteLine("existsunstripped" + existsunstripped);
                existsbepinex = true;
            }

            if (existsbepinex)
            {
                Console.WriteLine("\nThe following files/directories: BepInEx, doorstop_libs, unstripped_corlib, doorstop_config.ini or winhttp.dll\nwere found in the selected folder.");
                Console.WriteLine("Do you want to remove old BepInEx intallations? Everything will be deleted.");
                Console.WriteLine("[1] Yes");
                Console.WriteLine("[2] No");
                Console.Write("> ");
                int opcselec = int.Parse(Console.ReadLine());

                if (opcselec == 1)
                {
                    Console.WriteLine("Removing files...");
                    System.Threading.Thread.Sleep(2000);
                    
                    //Deleting Files
                    if(existsdoorstop)
                        File.Delete("doorstop_config.ini");
                    if(existswinhttp)
                        File.Delete("winhttp.dll");
                    if (existsbepfolder)
                    {
                        string bepinexdir = $"{Directory.GetCurrentDirectory()}/BepInEx";
                        DirectoryInfo directorybep = new DirectoryInfo(bepinexdir);
                        directorybep.Delete(true);
                    }
                    if (existsdorstoplibs)
                    {
                        string dorstopdir = $"{Directory.GetCurrentDirectory()}/doorstop_libs";
                        DirectoryInfo directorydorstop = new DirectoryInfo(dorstopdir);
                        directorydorstop.Delete(true);
                    }
                    if (existsunstripped)
                    {
                        string unstrippeddir = $"{Directory.GetCurrentDirectory()}/unstripped_corlib";
                        DirectoryInfo directoryunstrip = new DirectoryInfo(unstrippeddir);
                        directoryunstrip.Delete(true);
                    }
                    Console.WriteLine("\nFile removal successfully completed!");
                    System.Threading.Thread.Sleep(2000);
                }
                else if (opcselec == 2)
                {
                    Console.WriteLine("Okay, if you want to install the mod you must delete files from previous installations or select the \'Update Valheim+\' option.");
                    System.Threading.Thread.Sleep(8000);
                    InstallMenu();
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                    Console.WriteLine("[1] Yes");
                    Console.WriteLine("[2] No");
                    Console.Write("> ");
                    opcselec = int.Parse(Console.ReadLine());;
                }
                
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine("No previous installations were found, continuing...");
                System.Threading.Thread.Sleep(2000);
            }
        }

        public string SelectDirectory()
        {
            try
            {
                Menu();
                Console.WriteLine("First, choose the game directory");
                Console.WriteLine("Usually it stays at: C:/Program Files (x86)/Steam/steamapps/common/valheim");
                Console.WriteLine("We need you to write the complete directory, without errors.");
                Console.WriteLine("Which directory is your game in?\n");
                string internaldirselected = Console.ReadLine();
                SearchingDirectory(internaldirselected);
                Console.Clear();
                return internaldirselected;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid directory...");
                System.Threading.Thread.Sleep(2000);
                System.Environment.Exit(0);
                throw;
            }
        }

        public static void FinishThanks()
        {
            Console.WriteLine("========== Valheim+ Installer ==========\n");
            Console.WriteLine("ValheimPlus has been successfully installed! Welcome to ValheimPlus.");
            Console.WriteLine("The installer will shut down in 10 seconds...");
            Console.WriteLine("GitHub: http://github.valheim.plus/");
            Console.WriteLine("\nCreated by: CastBlacKing");
            System.Threading.Thread.Sleep(10000);
        }
        
        public static void Goodbye()
        {
            Console.WriteLine("========== Valheim+ Installer ==========\n");
            Console.WriteLine("ValheimPlus has been successfully uninstalled! Goodbye :(");
            Console.WriteLine("The installer will shut down in 10 seconds...");
            Console.WriteLine("GitHub: http://github.valheim.plus/");
            Console.WriteLine("\nCreated by: CastBlacKing");
            System.Threading.Thread.Sleep(10000);
        }
        
        public static void UpdateMessage()
        {
            Console.WriteLine("========== Valheim+ Installer ==========\n");
            Console.WriteLine("ValheimPlus has been successfully updated!");
            Console.WriteLine("The installer will shut down in 10 seconds...");
            Console.WriteLine("GitHub: http://github.valheim.plus/");
            Console.WriteLine("\nCreated by: CastBlacKing");
            System.Threading.Thread.Sleep(10000);
        }

        public static void InstallValheimPlus()
        {
            // Baixando o Core
            try
            {
                Program programa = new Program();
                string printdir = programa.dirselected;
                Console.WriteLine("DirSelected: " + printdir);
                SearchingBepInExInstall(programa.dirselected);
                WebClient webClient = new WebClient();
                Console.WriteLine("\nStarting Download....");
                webClient.DownloadFile("https://github.com/Valheim-Brasil/VPlus-Brasil/releases/latest/download/ValheimBrasil.zip", "ValheimBrasil.zip");
                Console.WriteLine("\nFile downloaded successfully! Extracting file into the game directory...");
                ZipFile.ExtractToDirectory("ValheimBrasil.zip", $"{Directory.GetCurrentDirectory()}");
            }
            catch (Exception)
            {
                Console.WriteLine("Core download and extraction failed...");
                System.Threading.Thread.Sleep(2000);
                System.Environment.Exit(0);
            }
            
            
            // Limpeza de Desnecessários
            System.Threading.Thread.Sleep(2500);
            Console.WriteLine("Deleting Core file .zip");
            File.Delete("ValheimBrasil.zip");
            System.Threading.Thread.Sleep(2000);
            Console.Clear();
        }

        public static void FullClean(string dir)
        {
            try
            {
                bool existsdoorstop = System.IO.File.Exists($"{dir}/doorstop_config.ini");
                bool existswinhttp = System.IO.File.Exists($"{dir}/winhttp.dll");
                bool existsbepfolder = System.IO.Directory.Exists($"{dir}/BepInEx");
                bool existsdorstoplibs = System.IO.Directory.Exists($"{dir}/doorstop_libs");
                bool existsunstripped = System.IO.Directory.Exists($"{dir}/unstripped_corlib");
                Console.WriteLine("Removing files...");
                System.Threading.Thread.Sleep(2000);
                
                //Deleting Files
                if(existsdoorstop)
                    File.Delete("doorstop_config.ini");
                if(existswinhttp)
                    File.Delete("winhttp.dll");
                if (existsbepfolder)
                {
                    string bepinexdir = $"{Directory.GetCurrentDirectory()}/BepInEx";
                    DirectoryInfo directorybep = new DirectoryInfo(bepinexdir);
                    directorybep.Delete(true);
                }
                if (existsdorstoplibs)
                {
                    string dorstopdir = $"{Directory.GetCurrentDirectory()}/doorstop_libs";
                    DirectoryInfo directorydorstop = new DirectoryInfo(dorstopdir);
                    directorydorstop.Delete(true);
                }
                if (existsunstripped)
                {
                    string unstrippeddir = $"{Directory.GetCurrentDirectory()}/unstripped_corlib";
                    DirectoryInfo directoryunstrip = new DirectoryInfo(unstrippeddir);
                    directoryunstrip.Delete(true);
                }
                Console.WriteLine("\nFile removal successfully completed!");
                System.Threading.Thread.Sleep(2000);
            }
            catch (System.Exception)
            {
                Console.WriteLine("File removal failed, restarting installer...");
                System.Threading.Thread.Sleep(2000);
                InstallMenu();
            }
        }

        //Função principal
        public static void Main(string[] args)
        {
            //Chamando o programa
            Program programa = new Program();
            //Tetando o padrão
            if(programa.TestDefaultDirectory())
            {
            }
            else
            { 
                programa.dirselected = programa.SelectDirectory();
            }

            //Menu de instalação
            InstallMenu();
            int index = int.Parse(Console.ReadLine());
            opcao opcaoSelecionada = (opcao)index;
            
            //Selector
            switch (opcaoSelecionada)
            {
             case opcao.ValheimPlus:
                 InstallValheimPlus();
                 FinishThanks();
                 break;
             case opcao.Desinstalar:
                 FullClean(programa.dirselected);
                 Console.Clear();
                 Goodbye();
                 break;
             case opcao.Atualizar:
                 // Atualizando o Brasil Mod
                 FullClean(programa.dirselected);
                 InstallValheimPlus();
                 UpdateMessage();
                 break;
            }
        }


    }
}