using Passero.Framework;
using System;
using System.Collections.Generic;

namespace Passero.Framework
{
    /// <summary>
    /// Wrapper class for ViewModel that contains both Model and DTO view models.
    /// </summary>
    /// <typeparam name="ModelClass">The type of the model class.</typeparam>
    /// <typeparam name="DTOClass">The type of the DTO class.</typeparam
    public class ViewModelWrapper<ModelClass, DTOClass>
    where ModelClass : class
    where DTOClass : class
    {
        public ViewModel<ModelClass> ModelViewModel { get; set; }
        public ViewModel<DTOClass> DTOViewModel { get; set; }
    
        public ViewModelWrapper(ViewModel<ModelClass> modelViewModel, ViewModel<DTOClass> dtoViewModel)
        {
        ModelViewModel = modelViewModel ?? throw new ArgumentNullException(nameof(modelViewModel));
        DTOViewModel = dtoViewModel ?? throw new ArgumentNullException(nameof(dtoViewModel));
    }

    // Esporre ModelItem di ModelViewModel
    public ModelClass ModelItem
    {
        get => ModelViewModel?.ModelItem;
        set
        {
            if (ModelViewModel != null)
            {
                ModelViewModel.ModelItem = value;
            }
        }
    }

        public ModelClass ModelItemShadow
        {
            get => ModelViewModel?.ModelItemShadow;
            set
            {
                if (ModelViewModel != null)
                {
                    ModelViewModel.ModelItemShadow = value;
                }
            }
        }
        // Esporre ModelItems di ModelViewModel
        public IList<ModelClass> ModelItems
        {
            get => ModelViewModel?.ModelItems;
            set
            {
                if (ModelViewModel != null)
                {
                    ModelViewModel.ModelItems = value;
                }
            }
        }

        public IList<ModelClass> ModelItemsShadow   
        {
            get => ModelViewModel?.ModelItemsShadow;
            set
            {
                if (ModelViewModel != null)
                {
                    ModelViewModel.ModelItemsShadow = value;
                }
            }
        }



        // Esporre ModelItem di DTOViewModel
        public DTOClass DTOItem
        {
            get => DTOViewModel?.ModelItem;
            set
            {
                if (DTOViewModel != null)
                {
                    DTOViewModel.ModelItem = value;
                }
            }
        }

        public DTOClass DTOItemShadow   
        {
            get => DTOViewModel?.ModelItemShadow;
            set
            {
                if (DTOViewModel != null)
                {
                    DTOViewModel.ModelItemShadow = value;
                }
            }
        }
        public IList<DTOClass> DTOItems
        {
            get => DTOViewModel?.ModelItems;
            set
            {
                if (DTOViewModel != null)
                {
                    DTOViewModel.ModelItems = value;
                }
            }
        }
        public IList<DTOClass> DTOItemsShadow
        {
            get => DTOViewModel?.ModelItemsShadow;
            set
            {
                if (DTOViewModel != null)
                {
                    DTOViewModel.ModelItemsShadow = value;
                }
            }
        }
        /// <summary>
        /// Moves to the first item in the ModelViewModel.
        /// </summary>
        /// <returns>An ExecutionResult indicating the success or failure of the operation.</returns>
        public ExecutionResult MoveFirstItem()
        {
            if (ModelViewModel != null)
            {
                return ModelViewModel.MoveFirstItem();
            }

            return new ExecutionResult
            {
                ResultCode = ExecutionResultCodes.Failed,
                ResultMessage = "ModelViewModel is not initialized."
            };
        }
    }
}