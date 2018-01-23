﻿using NormandErwan.MasterThesis.Experiment.Experiment.Task;
using NormandErwan.MasterThesis.Experiment.Inputs;
using NormandErwan.MasterThesis.Experiment.Inputs.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NormandErwan.MasterThesis.Experiment.Loggers
{
  public class ExperimentDetailsLogger : ExperimentBaseLogger
  {
    // Properties

    public Inputs.Cursor Index { get; set; }
    public ProjectedCursor ProjectedIndex { get; set; }
    public ProjectedCursor ProjectedThumb { get; set; }

    // Variables

    protected bool itemSelected, itemDeselected, itemMoved, itemClassified;
    protected Container selectedContainer;
    protected Item selectedItem;
    protected bool panning, zooming;
    protected Vector3 panningTranslation, zoomingTranslation;
    protected Vector3 zoomingScaling = Vector3.one;

    // MonoBehaviour methods

    protected virtual void LateUpdate()
    {
      if (IsConfigured && stateController.CurrentState.Id == stateController.taskTrialState.Id)
      {
        if (itemDeselected && !itemSelected && !itemMoved)
        {
          selectedItem = null;
          selectedContainer = null;
        }

        PrepareRow();

        AddToRow(Time.frameCount);
        AddToRow(deviceController.ParticipantId);
        AddToRow(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.fff"));

        AddToRow(grid.transform, false);
        AddToRow(grid.LossyScale);
        AddToRow(grid.IsConfigured);
        AddToRow(grid.IsCompleted);
        AddToRow(grid.DragToZoom);

        AddToRow(panning);
        AddToRow(panningTranslation);

        AddToRow(zooming);
        AddToRow(zoomingScaling);
        AddToRow(zoomingTranslation);

        AddToRow(itemSelected);
        AddToRow(itemDeselected);
        AddToRow(itemMoved);
        AddToRow(itemClassified);
        AddToRow(selectedContainer);
        AddToRow(selectedItem);

        AddToRow(Index.transform, false);
        AddToRow(ProjectedIndex.transform, false);
        AddToRow(ProjectedThumb.transform, false);
        AddToRow(head, false);
        AddToRow(mobileDevice, false);

        WriteRow();

        if (itemMoved || itemClassified)
        {
          selectedItem = null;
          selectedContainer = null;
        }
        itemSelected = itemDeselected = itemMoved = itemClassified = false;

        if (panning)
        {
          panning = false;
          panningTranslation = Vector3.zero;
        }
        if (zooming)
        {
          zooming = false;
          zoomingScaling = Vector3.one;
          zoomingTranslation = Vector3.zero;
        }
      }
    }

    // Methods

    public override void Configure()
    {
      Filename = "participant-" + deviceController.ParticipantId + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_details.csv";

      Columns = new List<string>() { "frame_id", "participant_id", "date_time" };

      AddTransformToColumns("grid");
      Columns.AddRange(new string[] { "grid_is_configured", "grid_is_completed", "zoom_button_is_pressed" });

      Columns.Add("panning");
      AddVector3ToColumns("panning_translation");

      Columns.Add("zooming");
      AddVector3ToColumns("zooming_scaling");
      AddVector3ToColumns("zooming_translation");

      Columns.AddRange(new string[] {
        "item_selected", "item_deselected", "item_moved", "item_classified",
        "selected_container", "selected_item"
      });

      AddTransformToColumns("index", false);
      AddTransformToColumns("projected_index", false);
      AddTransformToColumns("projected_thumb", false);
      AddTransformToColumns("head", false);
      AddTransformToColumns("phone", false);

      base.Configure();
    }

    protected override void Grid_Configured()
    {
    }

    protected override void Grid_Completed()
    {
    }

    protected override void Grid_ItemSelected(Container container, Item item)
    {
      if (item.IsSelected)
      {
        itemSelected = true;
        selectedContainer = container;
        selectedItem = item;
      }
      else
      {
        itemDeselected = true;
      }
    }

    protected override void Grid_ItemMoved(Container oldContainer, Container newContainer, Item item, Experiment.Task.Grid.ItemMovedType moveType)
    {
      itemMoved = true;
      if (moveType == Experiment.Task.Grid.ItemMovedType.Classified)
      {
        itemClassified = true;
      }
      selectedContainer = newContainer;
      selectedItem = item;
    }

    protected override void Grid_DraggingStarted(IDraggable grid)
    {
    }

    protected override void Grid_Dragging(IDraggable grid, Vector3 translation)
    {
      panning = true;
      panningTranslation = translation;
    }

    protected override void Grid_DraggingStopped(IDraggable grid)
    {
    }

    protected override void Grid_ZoomingStarted(IZoomable grid)
    {
    }

    protected override void Grid_Zooming(IZoomable grid, Vector3 scaling, Vector3 translation)
    {
      zooming = true;
      zoomingScaling = scaling;
      zoomingTranslation = translation;
    }

    protected override void Grid_ZoomingStopped(IZoomable grid)
    {
    }

    protected virtual void AddToRow(Container container)
    {
      if (container == null)
      {
        AddToRow("");
      }
      else
      {
        var position = grid.GetPosition(container);
        AddToRow("(" + position.x + ", " + position.y + ")");
      }
    }

    protected virtual void AddToRow(Item item)
    {
      AddToRow((item == null) ? "" : selectedContainer.Elements.IndexOf(item).ToString());
    }
  }
}