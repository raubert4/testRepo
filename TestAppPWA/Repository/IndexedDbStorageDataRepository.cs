using TestAppPWA.Models;
using System.Collections.Generic;
using TG.Blazor.IndexedDB;

namespace TestAppPWA.Repository
{
    public class IndexedDbStorageDataRepository : IDataRepository
    {
        #region Variables

        private readonly IndexedDBManager _indexedDBManager;

        public const string CST_Storename_UserSettings = "UserSettings";
        public const string CST_Storename_Races = "Races";

        public const string CST_Index_UniqueId = "uniqueId";
        public const string CST_Index_Id = "id";

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public IndexedDbStorageDataRepository(IndexedDBManager indexedDBManager)
        {
            _indexedDBManager = indexedDBManager;
        }

        #endregion

        #region Methods

        public async Task<UserSettings> GetUserSettingsAsync()
        {
            var res = await _indexedDBManager.GetRecords<UserSettings>(CST_Storename_UserSettings);
            var us = res.FirstOrDefault();
            if (us == null) us = new UserSettings() { UniqueId = Guid.NewGuid().ToString() };

            return us;
        }

        public async Task SetUserSettingsAsync(UserSettings userSettings)
        {
            await _indexedDBManager.ClearStore(CST_Storename_UserSettings);

            var record = new StoreRecord<UserSettings>
            {
                Storename = CST_Storename_UserSettings,
                Data = userSettings
            };

            await _indexedDBManager.UpdateRecord(record);
        }

        public async Task<List<Race>> GetRacesAsync()
        {
            var res = await _indexedDBManager.GetRecords<Race>(CST_Storename_Races);

            return res;
        }

        public async Task<Race?> GetRaceAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var res = await _indexedDBManager.GetRecordById<string, Race>(CST_Storename_Races, id);

            if (res?.Participants != null)
            {
                foreach (var participant in res.Participants)
                {
                    participant.Race = res;
                }
            }
            if (res?.Passings != null)
            {
                foreach (var pass in res.Passings)
                {
                    pass.Race = res;
                }
            }

            return res;
        }

        public async Task SetRaceAsync(Race race)
        {
            var record = new StoreRecord<Race>
            {
                Storename = CST_Storename_Races,
                Data = race
            };

            try
            {
                await _indexedDBManager.UpdateRecord(record);
            }
            catch (Exception ex) 
            {
                    ex.ToString();
            }
        }

        #endregion

        public static void CreateDatabase(IServiceCollection services)
        {
            services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = "AR2Timing.Mobile.PWA.Db";
                dbStore.Version = 1;

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = CST_Storename_UserSettings,
                    PrimaryKey = new IndexSpec { Name = CST_Index_UniqueId, KeyPath = CST_Index_UniqueId, Auto = false }
                });

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = CST_Storename_Races,
                    PrimaryKey = new IndexSpec { Name = CST_Index_Id, KeyPath = CST_Index_Id, Auto = false }
                });
            });
        }
    }
}
