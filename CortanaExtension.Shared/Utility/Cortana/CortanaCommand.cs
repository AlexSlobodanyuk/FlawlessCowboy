﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Media.SpeechRecognition;
using static CortanaExtension.Shared.Utility.FileHelper;

namespace CortanaExtension.Shared.Utility.Cortana
{
    public abstract class CortanaCommand
    {
        public const string Execute = "execute";
        public const string Notepad = "notepad";
        public const string YouTube = "youtube";
        public const string ToggleListening = "toggle_listening";
        public const string FeedMe = "feed_me";
        public const string Calibrate = "calibrate";
        public const string BriefMe = "brief_me";

        public string Name { get; set;  }
        public string Argument { get; set; }
        //
        public string RawText { get; private set; }
        public string Mode { get; private set; }

        protected CortanaCommand(string name, string argument, CommandDiagnostics diagnostics=null)
        {
            Name = name;
            Argument = argument;

            OrganizeFeedback(diagnostics);
        }

        protected CortanaCommand(string name)
        {
            Name = name;
        }

        public abstract Task Perform();

        protected string ProvideLaunchFeedback(string filename)
        {
            return "Launched " + filename + " !!!";
        }

        private void OrganizeFeedback(CommandDiagnostics diagnostics)
        {
            if (diagnostics != null) {
                RawText = diagnostics.RawText;
                Mode = diagnostics.Mode;
            }
            else {
                RawText = Mode = null;
            }
        }

        public string ToInputString()
        {
            string inputString = "cortana" + " " + Name + " " + Argument;
            return inputString;
        }

        /// <summary>
        /// Removes the "Execute" from "Execute Notepad", for instance
        /// </summary>
        public static string StripOffCommandName(string command, string textSpoken)
        {
            string[] words = textSpoken.Split(null); // split based on spaces
            string commandArg = "";
            foreach (string word in words)
            {
                if (!word.Equals(command))
                {
                    commandArg += word + " ";
                }
            }
            commandArg = commandArg.TrimEnd(' '); // eliminate extra space
            return commandArg;
        }

    }

    public class ExecuteCortanaCommand : CortanaCommand
    {
        public ExecuteCortanaCommand(string argument, CommandDiagnostics diagnostics = null) : base(Execute, argument, diagnostics) { /*Super constructor does everything*/ }

        public override async Task Perform()
        {
            string filename = Argument;
            string properFilename = FileHelper.ConvertToProperFilename(filename);
            await FileHelper.Launch(properFilename, StorageFolders.LocalFolder);
        }
    }

    public class ToggleListeningCortanaCommand : CortanaCommand
    {
        public ToggleListeningCortanaCommand(string argument, CommandDiagnostics diagnostics = null) : base(ToggleListening, argument, diagnostics) { /*Super constructor does everything*/ }

        public override async Task Perform()
        {
            string properFilename = "Cortana_Settings.ahk";
            await FileHelper.Launch(properFilename, StorageFolders.LocalFolder);
        }
    }

    public class NotepadCortanaCommand : CortanaCommand
    {
        public NotepadCortanaCommand(string argument, CommandDiagnostics diagnostics = null) : base(Notepad, argument, diagnostics) { /*Super constructor does everything*/ }

        public override async Task Perform()
        {
            string text = Argument;
            const string notepad = "Notepad";
            string properFilename = FileHelper.ConvertToProperFilename(notepad);
            ClipboardHelper.CopyToClipboard(text);
            await FileHelper.Launch(properFilename);
        }
    }

    public class YoutubeCortanaCommand : CortanaCommand
    {
        public YoutubeCortanaCommand(string argument, CommandDiagnostics diagnostics = null) : base(YouTube, argument, diagnostics) { /*Super constructor does everything*/ }

        public override async Task Perform()
        {
            string searchTarget = Argument;
            const string youtube = "YouTube";
            string properFilename = FileHelper.ConvertToProperFilename(youtube);
            ClipboardHelper.CopyToClipboard(searchTarget);
            await FileHelper.Launch(properFilename);
        }
    }

    public class FeedMeCortanaCommand : CortanaCommand
    {
        public FeedMeCortanaCommand(string argument, CommandDiagnostics diagnostics = null) : base(FeedMe, argument, diagnostics) { /*Super constructor does everything*/ }

        public override async Task Perform()
        {
            string properFilename = "Macro.ahk";
            await FileHelper.Launch(properFilename);
        }
    }

    public class CalibrateCortanaCommand : CortanaCommand
    {
        public CalibrateCortanaCommand(string argument, CommandDiagnostics diagnostics = null) : base(Calibrate, argument, diagnostics) { /*Super constructor does everything*/ }

        public override async Task Perform()
        {
            string properFilename = "Calibrate.ahk";
            await FileHelper.Launch(properFilename);
        }
    }

    public class BriefMeCortanaCommand : CortanaCommand
    {
        public BriefMeCortanaCommand(string argument, CommandDiagnostics diagnostics = null) : base(BriefMe, argument, diagnostics) { /*Super constructor does everything*/ }

        public override async Task Perform()
        {
            string properFilename = "Brief_Me.ahk";
            await FileHelper.Launch(properFilename);
        }
    }

}
