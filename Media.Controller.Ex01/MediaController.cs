﻿using Media.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Controller.Ex01
{
    /// <summary>
    /// An abstract base class for controllers who want to store and play media.
    /// </summary>
    public abstract class MediaController : IDisposable
    {
        protected IMapper _mediaMapper;

        /// <summary>
        /// A list where media items are stored.
        /// </summary>
        public List<DataModel.Media> List { get; protected set; } = new List<DataModel.Media>();
        /// <summary>
        /// A property where a selected media item can be stored.
        /// </summary>
        public DataModel.Media Selected { get; protected set; }

        /// <summary>
        /// Constructor that sets all properties to default (clearing selected) and loads data in the memory (List).
        /// </summary>
        public MediaController(IMapper mediaMapper)
        {
            _mediaMapper = mediaMapper;

            ClearSelected();
            InitializeData();
        }

        /// <summary>
        /// Loads data in the memory (List). This method is abstract because each derived classes will implement this differently.
        /// </summary>
        protected void InitializeData()
        {
            this.List.Clear();
            this.List.AddRange(_mediaMapper.GetAllMedia());
        }

        /// <summary>
        /// Sets the property Selected to default (null).
        /// </summary>
        public void ClearSelected()
        {
            Selected = null;
        }

        /// <summary>
        /// Sets the property Selected to an item from the list of media items.
        /// </summary>
        /// <param name="newSelected">An item from the list of media items you want to set as selected.</param>
        public void ChangeSelected(DataModel.Media newSelected)
        {
            if (List.Contains(newSelected))
            {
                Selected = newSelected;
            }
       }

        public DataModel.Media LoadMediaFile(int id)
        {
            foreach (DataModel.Media media in List)
            {
                if (media.Id == id)
                {
                    media.File = _mediaMapper.GetMediaFile(id);
                    return media;
                }
            }

            throw new DataModel.Exceptions.LoadMediaFileException();
        }

        /// <summary>
        /// Adds an item to the list of media items.
        /// </summary>
        /// <param name="newMedia">A new media item that you want to add to the list of media items.</param>
        public void AddMedia(DataModel.Media newMedia)
        {
            ClearSelected();
            //List.Add(newMedia);
            _mediaMapper.AddMedia(newMedia);
            this.InitializeData();
        }

        /// <summary>
        /// Removes an item from the list of media items.
        /// </summary>
        /// <param name="oldMedia">An item from the list of media items you want to delete.</param>
        public void RemoveMedia(DataModel.Media oldMedia)
        {
            ClearSelected();
            //List.Remove(oldMedia);
            _mediaMapper.DeleteMedia(oldMedia);
            this.InitializeData();
        }

        /// <summary>
        /// Updates an item in the list of media items.
        /// </summary>
        /// <param name="updateMedia">An item from the list of media items you want to update.</param>
        public void UpdateMedia(DataModel.Media updateMedia)
        {
            ClearSelected();
            _mediaMapper.UpdateMedia(updateMedia);
            this.InitializeData();
        }

        /// <summary>
        /// Builds a string that can be used in an OpenFileDialog as a filter for the type of files (extensions) you want to load.
        /// </summary>
        /// <returns>A string that can be passed to an OpenFileDialog as filter for the type of files you can add.</returns>
        public abstract string FileFilter();

        /// <summary>
        /// Disposes objects, that implement IDisposable, used in the controller or in a derived controller.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
