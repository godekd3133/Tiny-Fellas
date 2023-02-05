using System;

public class EnviromentUtils
{
    private static bool hadTriedToGetPort = false;
    private static int? port;

    private static bool hadTriedToGetLogFiledPath = false;
    private static string logFilePath;

    public static int? Port
    {
        get
        {
            if (hadTriedToGetPort == false)
            {
                port = ReadPortFromCmd();
                hadTriedToGetPort = true;
            }
            return port;
        }
    }

    public static string LogFilePath
    {
        get
        {
            if (hadTriedToGetLogFiledPath == false)
            {
                logFilePath = ReadLogFilePathFromCmd();
                hadTriedToGetLogFiledPath = true;
            }
            return logFilePath;
        }
    }
    private static int? ReadPortFromCmd()
    {
        string[] args = Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] != "-port")
            {
                continue;
            }

            if (!int.TryParse(args[i + 1], out int value))
            {
                continue;
            }

            if (value < 1000 || value >= 65536)
            {
                continue;
            }

            return value;
        }

        return null;
    }

    private static string ReadLogFilePathFromCmd()
    {
        string[] args = Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length - 2; i++)
        {
            if (args[i] != "-logFile")
            {
                continue;
            }

            return args[i + 1];
        }

        return null;
    }
}
