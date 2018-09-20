using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace xworks.taskprocess
{
	class TaskFile
	{
		public List<Task> LoadTasks(string filePath)
		{
            Program.Tasks  = new List<Task>();
            if (IsXml(filePath) == true)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlElement rootElem = doc.DocumentElement;
                XmlNodeList rowpersonnodes = rootElem.GetElementsByTagName("task");
                foreach (XmlElement node in rowpersonnodes)
                {
                    Task task = new Task();
                    if (CheckNodeExists(filePath, doc) == true)
                    {
                        if (Datecheck(node.SelectSingleNode("SubmitTime").InnerText) == true && Datecheck(node.SelectSingleNode("SubmitTime").InnerText) == true && Datecheck(node.SelectSingleNode("CheckTime").InnerText) == true&&Datecheck(node.SelectSingleNode("FinishTime").InnerText)==true)
                        {
                            if (Enum.IsDefined(typeof(TaskPriority), (TaskPriority)Enum.Parse(typeof(TaskPriority), node.SelectSingleNode("Priority").InnerText)) == true && Enum.IsDefined(typeof(TaskStatus), (TaskStatus)Enum.Parse(typeof(TaskStatus), node.SelectSingleNode("Status").InnerText)) == true)
                            {
                                task.Id = Guid.NewGuid().ToString();
                                task.Author = node.SelectSingleNode("Author").InnerText;
                                task.SubmitTime = DateTime.ParseExact(node.SelectSingleNode("SubmitTime").InnerText, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                task.Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), node.SelectSingleNode("Priority").InnerText);
                                task.DueTime = DateTime.ParseExact(node.SelectSingleNode("DueTime").InnerText, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                task.Assignee = node.SelectSingleNode("Assignee").InnerText;
                                task.Content = node.SelectSingleNode("Content").InnerText;
                                task.HandlingNote = node.SelectSingleNode("HandlingNote").InnerText;
                                task.Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), node.SelectSingleNode("Status").InnerText);
                                task.Checker = node.SelectSingleNode("Checker").InnerText;
                                task.CheckTime = DateTime.ParseExact(node.SelectSingleNode("CheckTime").InnerText, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                task.FinishTime = DateTime.ParseExact(node.SelectSingleNode("FinishTime").InnerText, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                               Program.Tasks.Add(task);
                            }
                            else
                            {
                                MessageBox.Show(node.Attributes["id"].Value + "枚举类型错误");
                            }
                        }
                        else
                        {
                            MessageBox.Show(node.Attributes["id"].Value + "时间格式错误");
                        }
                    }
                    else
                    {
                        MessageBox.Show(node.Attributes["id"].Value + "节点错误");
                    }
                }
                return Program.Tasks;
            }
            else
            {
                MessageBox.Show("文件格式不对");
                return Program.Tasks;
            }
		}
        
        bool Datecheck(string checkdate)
        {
            DateTime result=new DateTime();
            return DateTime.TryParseExact(checkdate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out result);
        }

        bool CheckNodeExists(string filePath, XmlDocument doc)
        {
            XmlElement rootElem = doc.DocumentElement;
            XmlNode id = rootElem.SelectSingleNode("task");
            XmlNode author = rootElem.SelectSingleNode("task/Author");
            XmlNode submittime = rootElem.SelectSingleNode("task/SubmitTime");
            XmlNode priority = rootElem.SelectSingleNode("task/Priority");
            XmlNode duetime = rootElem.SelectSingleNode("task/DueTime");
            XmlNode assignee = rootElem.SelectSingleNode("task/Assignee");
            XmlNode content = rootElem.SelectSingleNode("task/Content");
            XmlNode handlingnote = rootElem.SelectSingleNode("task/HandlingNote");
            XmlNode status = rootElem.SelectSingleNode("task/Status");
            XmlNode checker = rootElem.SelectSingleNode("task/Checker");
            XmlNode checktime = rootElem.SelectSingleNode("task/CheckTime");

            if (id.Attributes["id"]!=null&&author!=null&&submittime!=null&&priority!=null&&duetime!=null&&assignee!=null&&content!=null&&handlingnote!=null&&status!=null&&checker!=null&&checktime!=null)
            {
                return true;
            }else
            {
                return false;
            }
        }

        bool IsXml(string Path)
        {
            StreamReader sr = new StreamReader(Path);
            string strXml = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(strXml);
                return true;
            }
            catch
            {
                return false;

            }
        }

        public void Savefile(String filepath)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", ""));
            XmlElement root = doc.CreateElement("tasks");
            if (Program.Tasks == null)
            {
                MessageBox.Show("没有可保存的文件");
            }
            else
            {
                foreach (Task x in Program.Tasks)
                {
                    XmlElement node = doc.CreateElement("task");
                    node.SetAttribute("id", x.Id.ToString());
                    XmlElement author = doc.CreateElement("Author");
                    author.InnerText = x.Author;
                    node.AppendChild(author);
                    XmlElement submittime = doc.CreateElement("SubmitTime");
                    submittime.InnerText = x.SubmitTime.ToString("yyyyMMddHHmmss");
                    node.AppendChild(submittime);
                    XmlElement priority = doc.CreateElement("Priority");
                    priority.InnerText = ((int)Enum.Parse(typeof(TaskPriority), x.Priority.ToString())).ToString();
                    node.AppendChild(priority);
                    XmlElement duetime = doc.CreateElement("DueTime");
                    duetime.InnerText = x.DueTime.ToString("yyyyMMddHHmmss");
                    node.AppendChild(duetime);
                    XmlElement assignee = doc.CreateElement("Assignee");
                    assignee.InnerText = x.Assignee;
                    node.AppendChild(assignee);
                    XmlElement content = doc.CreateElement("Content");
                    content.InnerText = x.Content;
                    node.AppendChild(content);
                    XmlElement handlingnote = doc.CreateElement("HandlingNote");
                    handlingnote.InnerText = x.HandlingNote;
                    node.AppendChild(handlingnote);
                    XmlElement status = doc.CreateElement("Status");
                    status.InnerText = ((int)Enum.Parse(typeof(TaskStatus), x.Status.ToString())).ToString();
                    node.AppendChild(status);
                    XmlElement checker = doc.CreateElement("Checker");
                    checker.InnerText = x.Checker;
                    node.AppendChild(checker);
                    XmlElement checktime = doc.CreateElement("CheckTime");
                    checktime.InnerText = x.CheckTime.ToString("yyyyMMddHHmmss");
                    node.AppendChild(checktime);
                    XmlElement finishTime = doc.CreateElement("FinishTime");
                    finishTime.InnerText = x.FinishTime.ToString("yyyyMMddHHmmss");
                    node.AppendChild(finishTime);
                    root.AppendChild(node);
                }
            }
            doc.AppendChild(root);
            doc.Save(filepath);
        }

        public void Addtask(string priority,string duetime,string assignee,string content,string submittime)
        {
                Task task = new Task
                {
                    Id =Guid.NewGuid().ToString(),
                    Author = "张三",
                    SubmitTime = DateTime.ParseExact(submittime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                    Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), priority),
                    DueTime = DateTime.ParseExact(duetime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                    Assignee = assignee,
                    Content = content,
                    HandlingNote = "",
                    Status = (TaskStatus)Enum.Parse(typeof(TaskStatus), "0"),
                    Checker = "",
                    CheckTime = DateTime.ParseExact("19000101000000", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                    FinishTime = DateTime.ParseExact("19000101000000", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture)
                };
                Program.Tasks.Add(task);
            }
       
        
        public void Delet(string id)
        {
            for(int i = Program.Tasks.Count - 1; i >= 0; i--)
            {
                if (Program.Tasks[i].Id == id)
                {
                    Program.Tasks.Remove(Program.Tasks[i]);
                }
            }
        }

        public void Update(string id, string priority, string duetime, string assignee, string content)
        {
            foreach(Task x in Program.Tasks)
            {
                if (x.Id == id)
                {
                    x.Priority = (TaskPriority)Enum.Parse(typeof(TaskPriority), priority);
                    x.DueTime = DateTime.ParseExact(duetime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    x.Assignee = assignee;
                    x.Content = content;
                }
            }
        }

        public void UpdatePriority(string id, TaskPriority priority)
        {

            foreach (Task x in Program.Tasks)
            {
                if (x.Id == id)
                {
                    x.Priority = priority;
                }
            }
        }
        
    }
}
