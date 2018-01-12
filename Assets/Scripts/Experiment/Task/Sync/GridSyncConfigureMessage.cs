﻿using DevicesSyncUnity.Messages;
using System;

namespace NormandErwan.MasterThesis.Experiment.Experiment.Task.Sync
{
  public class GridSyncConfigureMessage : DevicesSyncMessage
  {
    // Constructors and destructor

    public GridSyncConfigureMessage(Grid grid, Action sendToServer) : base()
    {
      Grid = grid;
      SendToServer = sendToServer;

      grid.ConfigureSync += Grid_ConfigureSync;
    }

    public GridSyncConfigureMessage() : base()
    {
    }

    ~GridSyncConfigureMessage()
    {
      Grid.ConfigureSync -= Grid_ConfigureSync;
    }

    // Properties

    public override int SenderConnectionId { get { return senderConnectionId; } set { senderConnectionId = value; } }
    public override short MessageType { get { return MasterThesis.Experiment.MessageType.GridConfigure; } }

    protected Grid Grid { get; }
    protected Action SendToServer { get; }

    // Variables

    public int senderConnectionId;

    public int columnsNumber, rowsNumber, itemsPerContainer;
    public int[] itemValues;

    // Methods

    public void ConfigureGrid(Grid grid)
    {
      var containers = new GridGenerator.Container[rowsNumber, columnsNumber];
      IterateContainers(rowsNumber, columnsNumber, itemsPerContainer, (row, col, itemIndex, valueIndex) =>
      {
        if (itemIndex == 0)
        {
          containers[row, col].items = new int[itemsPerContainer];
        }
        containers[row, col].items[itemIndex] = itemValues[valueIndex];
      });

      var gridGenerator = new GridGenerator(containers);
      grid.SetConfiguration(gridGenerator);
    }

    protected void Grid_ConfigureSync(GridGenerator gridGenerator)
    {
      columnsNumber = gridGenerator.ColumnsNumber;
      rowsNumber = gridGenerator.RowsNumber;
      itemsPerContainer = gridGenerator.ItemsPerContainer;

      itemValues = new int[rowsNumber * columnsNumber * itemsPerContainer];
      IterateContainers(rowsNumber, columnsNumber, itemsPerContainer, (row, col, itemIndex, valueIndex) =>
      {
        itemValues[valueIndex] = gridGenerator.Containers[row, col].items[itemIndex];
      });

      SendToServer();
    }

    protected void IterateContainers(int rowsNumber, int columnsNumber, int itemsPerContainer, Action<int, int, int, int> onEachItem)
    {
      for (int row = 0; row < rowsNumber; ++row)
      {
        for (int col = 0; col < columnsNumber; ++col)
        {
          for (int itemIndex = 0; itemIndex < itemsPerContainer; ++itemIndex)
          {
            var valueIndex = row * columnsNumber * itemsPerContainer + col * itemsPerContainer + itemIndex;
            onEachItem(row, col, itemIndex, valueIndex);
          }
        }
      }
    }
  }
}
