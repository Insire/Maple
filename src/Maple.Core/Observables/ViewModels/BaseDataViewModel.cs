﻿using System;
using System.Runtime.CompilerServices;

namespace Maple.Core
{
    public abstract class BaseDataViewModel<TModel> : BaseViewModel<TModel>
        where TModel : class
    {
        protected ChangeTracker ChangeTracker { get; }
        protected bool SkipChangeTracking { get; set; }

        public bool IsChanged
        {
            get { return ChangeTracker.HasChanged; }
        }

        protected BaseDataViewModel(TModel model, IMessenger messenger)
            : base(model, messenger)
        {
            SkipChangeTracking = true;

            ChangeTracker = new ChangeTracker();
            Model = model ?? throw new ArgumentNullException(nameof(model));

            SkipChangeTracking = false;
        }

        protected override bool SetValue<T>(ref T field, T value, Action OnChanging = null, Action OnChanged = null, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetValue(ref field, value, OnChanging, OnChanged, propertyName);

            if (result && !SkipChangeTracking && ChangeTracker.Update(value, propertyName))
            {
                OnPropertyChanged(nameof(IsChanged));
            }

            return result;
        }
    }
}
