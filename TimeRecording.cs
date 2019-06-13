using System;
using SQLite;

namespace Zewis
{
    //Datenmodell Arbeitszeit
    public class TimeRecording
    {
        [PrimaryKey][AutoIncrement]
        //Primärschlüssel
        public int Id { get; set; }
        public string ProjectName { get; set; }
        //Fremdschlüssel(zeigt auf Activity)
        public int Aid { get; set; }
        public long Date { get; set; }
        public double Time { get; set; }
        public bool Invoiceable { get; set; }
    }
}
