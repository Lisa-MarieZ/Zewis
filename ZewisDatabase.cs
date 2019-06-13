using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace Zewis
{
    //Datenbank Klasse
    public class ZewisDatabase
    {
        readonly SQLiteAsyncConnection database;

        //Konstruktor
        public ZewisDatabase(string dbPath)
        {
            //Erzeugt Datenbank in angegebenen Pfad
            database = new SQLiteAsyncConnection(dbPath);
            //Erzeugt Activitäten Tabelle
            database.CreateTableAsync<Activity>().Wait();
            //Erzeugt Arbeitszeiten Tabelle
            database.CreateTableAsync<TimeRecording>().Wait();

            //automatisiertes Befüllen der Aktivitäten Tabelle
            database.InsertAsync(new Activity()
            {
                Id = 1016,
                Name = "Anforderunganalyse"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1009,
                Name = "Audit / Assessment"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1209,
                Name = "Ausgleich/ Ruhezeit"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1011,
                Name = "Besprechungen"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1208,
                Name = "bezahlte Freistellung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1000,
                Name = "Consulting"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1013,
                Name = "Einarbeitung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1001,
                Name = "Entwicklung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1018,
                Name = "Entwicklungsbeauftragung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1211,
                Name = "Interne Tätigkeit"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1201,
                Name = "Jahresurlaub"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1007,
                Name = "Konzeption"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1203,
                Name = "Krankheit"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1012,
                Name = "Lehrlingsausbildung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1102,
                Name = "Messe"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1017,
                Name = "Modellierung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1006,
                Name = "Projekt- Leitung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1005,
                Name = "Projekt- Management"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1210,
                Name = "Projektvorbereitung"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1010,
                Name = "QS"
            });
            database.InsertAsync(new Activity()
            {
                Id = 1108,
                Name = "Reisezeit aktiv"
            }); database.InsertAsync(new Activity()
            {
                Id = 1008,
                Name = "Reisezeit passiv"
            }); database.InsertAsync(new Activity()
            {
                Id = 1003,
                Name = "Schulung"
            }); database.InsertAsync(new Activity()
            {
                Id = 1205,
                Name = "Seminar/ Fortbildungt"
            }); database.InsertAsync(new Activity()
            {
                Id = 1014,
                Name = "Service"
            }); database.InsertAsync(new Activity()
            {
                Id = 1202,
                Name = "Sonderurlaub"
            }); database.InsertAsync(new Activity()
            {
                Id = 1002,
                Name = "Support"
            }); database.InsertAsync(new Activity()
            {
                Id = 1021,
                Name = "Systemtest"
            }); database.InsertAsync(new Activity()
            {
                Id = 1015,
                Name = "Testen"
            }); database.InsertAsync(new Activity()
            {
                Id = 1020,
                Name = "Testfallerstellung"
            }); database.InsertAsync(new Activity()
            {
                Id = 1019,
                Name = "Testkonzeption"
            }); database.InsertAsync(new Activity()
            {
                Id = 1206,
                Name = "Unbezahlte Freistellung, Kind krank"
            }); database.InsertAsync(new Activity()
            {
                Id = 1207,
                Name = "Unbezahlte Freistellung, weitere Gründe"
            }); database.InsertAsync(new Activity()
            {
                Id = 1022,
                Name = "Unterstützung Abnahmetest"
            }); database.InsertAsync(new Activity()
            {
                Id = 1200,
                Name = "Urlaub Langzeitkonto"
            }); database.InsertAsync(new Activity()
            {
                Id = 1101,
                Name = "Vertriebsunterstützung"
            }); database.InsertAsync(new Activity()
            {
                Id = 1004,
                Name = "Workshop"
            });
        }

        //Methode zum Holen der Aktivitäten
        public Task<List<Activity>> GetActivitiesAsync()
        {
            return database.Table<Activity>().ToListAsync();
        }

        //Methode zum Holen der Aktivität anhand des Namens
        public int GetActivityIdAsync(String name)
        {
            var getIdTask = database.QueryAsync<Activity>("SELECT * FROM [Activity] WHERE [Name] = '" + name + "'");
            var id = getIdTask.Result[0].Id;
            return id;
        }

        //Methode zum Holen der Monatsübersicht
        public Task<List<TimeRecording>> GetMonthViewAsync(int month)
        {
            return database.QueryAsync<TimeRecording>("SELECT * FROM [TimeRecording] WHERE [Date.Month] = " + month);
        }

        //Methode zum Holen der Arbeitszeiten Tagesübersicht(nicht verwendet)
        public Task<List<TimeRecording>> GetDayViewAsync(DateTime date)
        {
            return database.QueryAsync<TimeRecording>("SELECT * FROM [TimeRecording] WHERE [Date] = " + date);
        }

        //Methode zum Holen der Summe der Stunden eines Tages(nicht verwendet)
        public string GetDaySumAsync(DateTime date)
        {
            return database.QueryAsync<TimeRecording>("SELECT SUM(Time) AS sumtime FROM [TimeRecording] WHERE [Date] = " + date).ToString();
        }

        //Methode zum Holen der Arbeitszeiten Tagesübersicht
        public Task<List<TimeRecording>> GetTimeRecordingAsync(DateTime date)
        {
            return database.QueryAsync<TimeRecording>("SELECT * FROM [TimeRecording] WHERE [Date] = " + date.Ticks);
        }

        //Mehtode zum Holen aller Arbeitszeiten(noch nicht verwendet)
        public Task<List<TimeRecording>> GetAllTimeRecordingsAsync()
        {
            return database.QueryAsync<TimeRecording>("SELECT * FROM [TimeRecording]");
        }

        //Methode zum Speichern einer Arbeitszeit
        public Task<int> saveTimeRecordingAsync(DateTime date, string projectName, int aid ,double time, bool invoiceable)
        {
            return database.InsertAsync(new TimeRecording()
            {
                ProjectName = projectName,
                Aid = aid,
                Date = date.Ticks,
                Time = time, 
                Invoiceable = invoiceable
            });
        }
    }
}
