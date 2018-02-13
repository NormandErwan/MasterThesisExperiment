﻿using System;

namespace NormandErwan.MasterThesis.Experiment.Inputs.Interactables
{
  public interface IInteractable
  {
    // Properties

    bool IsInteractable { get; }
    int Priority { get; }

    // Events

    event Action<IInteractable> Interactable;

    // Methods

    void SetInteractable(bool value);
  }
}