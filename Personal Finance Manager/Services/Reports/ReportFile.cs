namespace Personal_Finance_Manager.Services.Reports
{
    public class ReportFile
    {
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }


        public ReportFile(byte[] bytes, string contentType, string fileName)
        {
            Bytes = bytes;
            ContentType = contentType;
            FileName = fileName;
        }
    }
}
