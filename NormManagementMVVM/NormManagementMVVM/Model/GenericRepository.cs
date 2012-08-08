using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using SharedComponents.Connection;
using SharedComponents.Linq.Specification;

namespace NormManagementMVVM.Model
{
    /// <summary>
    /// Generic repository
    /// </summary>
    public class GenericRepository // : IRepository
    {
        //private  readonly string _connectionStringName;
        private static NormEntities _objectContext;
        private static IUnitOfWork _unitOfWork;

        public static IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork(ObjectContext)); }
        }

        private static NormEntities ObjectContext
        {
            get { return _objectContext; }
        }

        public static void InitializeServer()
        {
            _objectContext = new NormEntities(BuildConnectionString());
            _objectContext.Y_NORM_MANAGEMENT_INIT_PROFILE_ITEM_STORE_PARAM();
        }

        public static string BuildConnectionString()
        {
            const string providerName = "Oracle.DataAccess.Client";
            //const string serverName = "(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.0.1.15)(PORT = 1530)))(CONNECT_DATA =(SID = RMSP)));PASSWORD=golive104;PERSIST SECURITY INFO=True;USER ID=RMSPRD";
            //string serverName = RMSConnection.Current.GetConnectionString(User.Name, User.Password);
            string serverName = RMSConnection.Current.GetConnectionString(User.Name, User.Password);
            const string metadata =
                "res://*/Model.NormativeModel.csdl|res://*/Model.NormativeModel.ssdl|res://*/Model.NormativeModel.msl";
            string entBild = string.Format("metadata={0};provider={1};provider connection string=\";{2}\"", metadata,
                                           providerName, serverName);
            return entBild;
        }

        public static void RollbackContext()
        {
            foreach (ObjectStateEntry entry in
                _objectContext.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Modified | EntityState.Deleted))
            {
                _objectContext.Refresh(RefreshMode.StoreWins, entry.Entity);
            }
            foreach (ObjectStateEntry entry in
                _objectContext.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added))
            {
                if (entry.State == EntityState.Detached) continue;
                _objectContext.DeleteObject(entry.Entity);
            }
        }

        public static TEntity GetByKey<TEntity>(object keyValue) where TEntity : class
        {
            EntityKey key = GetEntityKey<TEntity>(keyValue);

            object originalItem;
            if (ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (TEntity) originalItem;
            }
            return default(TEntity);
        }

        public static IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            string entityName = GetEntityName<TEntity>();
            return ObjectContext.CreateQuery<TEntity>(entityName);
        }

        public static IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return GetQuery<TEntity>().Where(predicate);
        }

        public static IQueryable<TEntity> GetQuery<TEntity>(ISpecification<TEntity> specification) where TEntity : class
        {
            return specification.SatisfyingEntitiesFrom(GetQuery<TEntity>());
        }

        public static IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, string>> orderBy, int pageIndex,
                                                        int pageSize, SortOrder sortOrder = SortOrder.Ascending)
            where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>().OrderBy(orderBy).Skip(pageIndex).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>().OrderByDescending(orderBy).Skip(pageIndex).Take(pageSize).AsEnumerable();
        }

        public static IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate,
                                                        Expression<Func<TEntity, string>> orderBy, int pageIndex,
                                                        int pageSize, SortOrder sortOrder = SortOrder.Ascending)
            where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return
                    GetQuery<TEntity>().Where(predicate).OrderBy(orderBy).Skip(pageIndex).Take(pageSize).AsEnumerable();
            }
            return
                GetQuery<TEntity>().Where(predicate).OrderByDescending(orderBy).Skip(pageIndex).Take(pageSize).
                    AsEnumerable();
        }

        public static IEnumerable<TEntity> Get<TEntity>(ISpecification<TEntity> specification,
                                                        Expression<Func<TEntity, string>> orderBy, int pageIndex,
                                                        int pageSize, SortOrder sortOrder = SortOrder.Ascending)
            where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return
                    specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderBy(orderBy).Skip(pageIndex).Take(
                        pageSize).AsEnumerable();
            }
            return
                specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderByDescending(orderBy).Skip(pageIndex).
                    Take(pageSize).AsEnumerable();
        }

        public static TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().SingleOrDefault<TEntity>(criteria);
        }

        public static TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(GetQuery<TEntity>());
        }

        public static TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return GetQuery<TEntity>().FirstOrDefault(predicate);
        }

        public static TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).FirstOrDefault();
        }

        public static void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            ObjectContext.AddObject(GetEntityName<TEntity>(), entity);
        }

        public static void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            ObjectContext.AttachTo(GetEntityName<TEntity>(), entity);
        }

        public static void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            ObjectContext.DeleteObject(entity);
        }

        public static void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            IEnumerable<TEntity> records = Find(criteria);

            foreach (TEntity record in records)
            {
                Delete(record);
            }
        }

        public static void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            IEnumerable<TEntity> records = Find(criteria);
            foreach (TEntity record in records)
            {
                Delete(record);
            }
        }

        public static IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return GetQuery<TEntity>().AsEnumerable();
        }

        public static List<TEntity> GetAllList<TEntity>() where TEntity : class
        {
            return GetQuery<TEntity>().ToList();
        }

        public static string GetParameterNames(long? iParamId, string iParamValues)
        {
            return _objectContext.GetParameterNames(iParamId, iParamValues);
        }

        public static ObservableCollection<TEntity> GetAllObservableCollection<TEntity>() where TEntity : class
        {
            return GetQuery<TEntity>().ToObservableCollection();
        }

        public static void Update<TEntity>(TEntity entity) where TEntity : class
        {
            string fqen = GetEntityName<TEntity>();

            object originalItem;
            EntityKey key = ObjectContext.CreateEntityKey(fqen, entity);
            if (ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                ObjectContext.ApplyCurrentValues(key.EntitySetName, entity);
            }
        }

        public static IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Where(criteria);
        }

        public static TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Where(criteria).FirstOrDefault();
        }

        public static TEntity FindOne<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntityFrom(GetQuery<TEntity>());
        }

        public static IEnumerable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>());
        }

        public static int Count<TEntity>() where TEntity : class
        {
            return GetQuery<TEntity>().Count();
        }

        public static int Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            return GetQuery<TEntity>().Count(criteria);
        }

        public static int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).Count();
        }

        private static EntityKey GetEntityKey<TEntity>(object keyValue) where TEntity : class
        {
            string entitySetName = GetEntityName<TEntity>();
            ObjectSet<TEntity> objectSet = ObjectContext.CreateObjectSet<TEntity>();
            string keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            var entityKey = new EntityKey(entitySetName, new[] {new EntityKeyMember(keyPropertyName, keyValue)});
            return entityKey;
        }

        public static ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> GetParamValues(int paramId,
                                                                                                         string
                                                                                                             paramClause,
                                                                                                         int profileId)
        {
            return _objectContext.GetParameterValues(paramId, paramClause, profileId);
        }

        public static ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> GetParamValues(int paramId)
        {
            return _objectContext.GetParameterValues(paramId, "", 0);
        }

        public static ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> GetValues(int paramId,
                                                                                                    string paramValues)
        {
            if (paramId == 0) return null;
            return _objectContext.GetValues(paramId, paramValues);
        }

        public static List<PivotRow> GetPivotRows()
        {
            return _objectContext.GetPivotParameters();
        }

        private static string GetEntityName<TEntity>() where TEntity : class
        {
            return string.Format("{0}.{1}", ObjectContext.DefaultContainerName, typeof (TEntity).Name);
        }

        public static bool GetChanges()
        {
            bool temp = false;
            if (_objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
                                                                        EntityState.Deleted).Count() <= 2)
            {
                if (_objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
                                                                            EntityState.Deleted).Count() != 0)
                {
                    foreach (
                        ObjectStateEntry entry in
                            _objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added |
                                                                                    EntityState.Modified).Where(
                                                                                        entry =>
                                                                                        !entry.IsRelationship &&
                                                                                        (entry.Entity.GetType() ==
                                                                                         typeof (Y_NORM_NORMATIVE_ROW)))
                        )
                    {
                        if (((Y_NORM_NORMATIVE_ROW) entry.Entity).Y_NORM_NORMATIVE_CELL.Count == 0)
                        {
                            _objectContext.DeleteObject(entry.Entity);
                            temp = false;
                        }
                        else
                        {
                            temp = true;
                        }
                    }
                    if (!temp) RollbackContext();
                }
            }
            else
            {
                temp = true;
            }
            return temp;
        }
    }
}