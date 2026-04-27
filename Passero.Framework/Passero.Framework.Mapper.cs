using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Passero.Framework.Base
{
    public interface IMapper<ModelClass, DTOClass>
        where ModelClass : ModelBase
        where DTOClass : DTOBase<ModelClass>
    {
        IList<DTOClass> MapToDTO(IList<ModelClass> modelItems);
        IList<ModelClass> MapToModel(IList<DTOClass> dtoItems);
    }

    public class DefaultMapper<ModelClass, DTOClass> : IMapper<ModelClass, DTOClass>
       where ModelClass : ModelBase, new()
       where DTOClass : DTOBase<ModelClass>, new()
    {
        public IList<DTOClass> MapToDTO(IList<ModelClass> modelItems)
        {
            return modelItems.Select(model => MapProperties(new DTOClass(), model)).ToList();
        }

        public IList<ModelClass> MapToModel(IList<DTOClass> dtoItems)
        {
            return dtoItems.Select(dto => MapProperties(new ModelClass(), dto)).ToList();
        }

        private TTarget MapProperties<TTarget, TSource>(TTarget target, TSource source)
        {
            var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var targetProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProp in sourceProperties)
            {
                var targetProp = targetProperties.FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
                if (targetProp != null && targetProp.CanWrite)
                {
                    targetProp.SetValue(target, sourceProp.GetValue(source));
                }
            }

            return target;
        }
    }
}