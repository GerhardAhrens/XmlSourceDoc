#pragma warning disable CS1591

namespace XmlSourceDocDemo
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class InMemoryRepository<TDomain> : IRepository<TDomain> where TDomain : IDomainRoot
    {
        private readonly ConcurrentDictionary<Type, TDomain> memorySource = null;
        public InMemoryRepository()
        {
            this.memorySource = new ConcurrentDictionary<Type, TDomain>();
        }

        public TDomain FindById(Guid id)
        {
            TDomain result = default(TDomain);

            if (this.memorySource != null)
            {
                if (this.memorySource.Any(a => a.Value.Id == id))
                {
                    result = (TDomain)this.memorySource.Where(w => w.Value.Id == id);
                }
            }

            return result;
        }

        public void Add(TDomain domainObj)
        {
            if (this.memorySource != null)
            {
                Type typ = typeof(TDomain);
                this.memorySource.TryAdd(typ, domainObj);
            }
        }

        public void Update(TDomain domainObj)
        {
            if (this.memorySource != null)
            {
                Type typ = typeof(TDomain);
                TDomain oldDomain = this.memorySource.Where(w => w.Value.Id == domainObj.Id).FirstOrDefault().Value;
                this.memorySource.TryUpdate(typ, domainObj, (TDomain)oldDomain);
            }
        }

        public void Remove(TDomain domainObj)
        {
            if (this.memorySource != null)
            {
            }
        }
    }

    public interface IDomainRoot
    {
        Guid Id { get; }
    }

    public interface IRepository<TDomain> where TDomain : IDomainRoot
    {
        TDomain FindById(Guid id);

        void Add(TDomain domainObj);

        void Update(TDomain domainObj);

        void Remove(TDomain domainObj);
    }
}
