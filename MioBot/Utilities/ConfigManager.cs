using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MioBot.Utilities
{
    public class ConfigurationManager
    {
        private static string configurationFromat = "{{\"ProjectId\":\"{0}\",\"PredictionKey\":\"{1}\"}";
        private string configuration;
        string fileName = "configuration.json";
        string filePath = "";

        private string projectId = "";      
        private string predictionKey = "";     

        // Use this for initialization
        void Start()
        {
            configuration = string.Format(configurationFromat, projectId, predictionKey);
            if (File.Exists(filePath))
            {       
                configuration = ReadConfigurationFile();
            }
            else
            {
                CreateConfigurationFile();
            }
            ParseConfiguration();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Create configuration file
        /// </summary>
        public void CreateConfigurationFile()
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Seek(0, SeekOrigin.Begin);
                byte[] data = new UTF8Encoding().GetBytes(configuration);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Dispose();
            }
        }

        /// <summary>
        /// Parse configuration data
        /// </summary>
        public string ReadConfigurationFile()
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                int fileLength = (int)fs.Length;

                if (fileLength == 0)
                {
                    byte[] data = new UTF8Encoding().GetBytes(configuration);
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                    fs.Dispose();
                    return configuration;
                }
                else
                {
                    byte[] data = new byte[fileLength];
                    fs.Read(data, 0, fileLength);
                    string jsonStr = Encoding.UTF8.GetString(data);
                    return jsonStr;
                }
            }
        }

        /// <summary>
        /// Update config data file
        /// </summary>
        public void UpdateConfiguration(string projectId, string predictionKey)
        {
            this.projectId = projectId;
            this.predictionKey = predictionKey;
         
            configuration = string.Format(configurationFromat, this.projectId, this.predictionKey);
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                int fileLength = (int)fs.Length;

                fs.SetLength(0);
                byte[] data = new UTF8Encoding().GetBytes(configuration);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Dispose();
            }
        }

        /// <summary>
        /// Parse data
        /// </summary>
        private void ParseConfiguration()
        {
        }

        public string GetProjectId()
        {
            return projectId;
        }

        public string GetPredictionKey()
        {
            return predictionKey;
        }
    }
}
