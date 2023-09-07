﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesCrane.Model
{
    public class Game
    {
        private string _title;
        private string _path;
        private string _imagePath;

        private int _numIndex;
        private int[] _vendIndex;

        private bool _needsAdmin;
        private bool _hasFlags;

        public Game() 
        {
            _title = "";
            _path = "";
            _imagePath = "";

            _numIndex = -2;
            _vendIndex = new int[2];

            _needsAdmin = false;
            _hasFlags = false;
        }

        public Game(Game other)
        {
            _title = other.Title;
            _path = other.Path;
            _imagePath = other.ImagePath;

            _numIndex = other.NumIndex; 
            _vendIndex = other.VendIndex;

            _needsAdmin = other.NeedsAdmin;
            _hasFlags = other.HasFlags; 
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
            }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
            }
        }

        public int NumIndex
        {
            get { return _numIndex; }
            set
            {
                _numIndex = value;
            }
        }

        public int[] VendIndex
        {
            get { return _vendIndex; }
            set
            {
                _vendIndex = value;
            }
        }


        public bool NeedsAdmin
        {
            get
            {
                return _needsAdmin;
            }
            set
            {
                _needsAdmin = value;
            }
        }

        public bool HasFlags 
        { 
            get 
            { 
                return _hasFlags; 
            }
            set
            {
                _hasFlags = value;
            }
        }
    }
}