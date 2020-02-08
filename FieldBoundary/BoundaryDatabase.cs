using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace FieldBoundary
{
    class BoundaryDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public BoundaryDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Feature).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Feature)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<Feature>> GetItemsAsync()
        {
            return Database.Table<Feature>().ToListAsync();
        }

        public Task<List<Feature>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<Feature>("SELECT * FROM [Feature] WHERE [Done] = 0");
        }

        public Task<Feature> GetItemAsync(int id)
        {
            return Database.Table<Feature>().Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Feature feature)
        {
            if (feature.id != 0)
            {
                return Database.UpdateAsync(feature);
            }
            else
            {
                return Database.InsertAsync(feature);
            }
        }

        public Task<int> DeleteItemAsync(Feature feature)
        {
            return Database.DeleteAsync(feature);
        }
    }
}
