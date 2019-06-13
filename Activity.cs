using System;
using SQLite;

namespace Zewis
{   //Datenmodell für Aktivitäten
    public class Activity
    {
        [PrimaryKey]
        [Unique]
        //Primärschlüssel
        public int Id { get; set; }
        //Attribut
        public string Name { get; set; }
    }
}
