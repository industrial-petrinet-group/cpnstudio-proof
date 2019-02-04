using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized; 
using IPG.CPNStudio.Diagram.Utils;
using Microsoft.Msagl.Core.Layout;

namespace IPG.CPNStudio.Demo.ViewModel
{
    /// <summary>
    /// Defines a network of nodes and connections between the nodes.
    /// </summary>
    public sealed class NetworkViewModel : GeometryGraph
    {
        #region Internal Data Members

        /// <summary>
        /// The collection of nodes in the network.
        /// </summary>
        private ImpObservableCollection<NodeViewModel> nodes = null;

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        private ImpObservableCollection<ConnectionViewModel> connections = null;

        #endregion Internal Data Members

        /// <summary>
        /// The collection of nodes in the network.
        /// </summary>
        public ImpObservableCollection<NodeViewModel> NodesSource
        {
            get
            {
                if (nodes == null)
                {
                    nodes = new ImpObservableCollection<NodeViewModel>();
                }

                return nodes;
            }
        }

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        public ImpObservableCollection<ConnectionViewModel> Connections
        {
            get
            {
                if (connections == null)
                {
                    connections = new ImpObservableCollection<ConnectionViewModel>();
                    connections.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(connections_ItemsRemoved);
                }

                return connections;
            }
        }

        #region Private Methods

        /// <summary>
        /// Event raised then Connections have been removed.
        /// </summary>
        private void connections_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectionViewModel connection in e.Items)
            {
                connection.SourceConnector = null;
                connection.DestConnector = null;
            }
        }

        public  void AddNode(NodeViewModel node)
        {
            this.NodesSource.Add(node);
            this.Nodes.Add(node);
        }

        internal void AddConnection(ConnectionViewModel connectionViewModel)
        {
            this.Connections.Add(connectionViewModel);
            this.Edges.Add(connectionViewModel);
        }

        #endregion Private Methods
    }
}
