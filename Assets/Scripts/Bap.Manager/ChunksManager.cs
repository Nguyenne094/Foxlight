using System;
using System.Collections.Generic;
using System.Linq;
using PlatformingGame.Controller;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Manage chunks's behaviour
    /// </summary>
    public class ChunksManager : MonoBehaviour
    {
        //TODO: enables 3 chunks around the player, diables other chunks
        //*IDEA: Initialize a chunk list and just show 2 chunk around.
        //*Example: Player is running on chunk 4 and system shows chunk 3, 4, 5

        private List<Chunk> _chunks;
        private List<Chunk> _activeChunks;
        private int _prevIndex;
        private int _subIndex;
        private int _currentIndex;

        private void Awake()
        {
            _chunks = GetComponentsInChildren<Chunk>().ToList();
        }

        private void Start()
        {
            foreach (var chunk in _chunks)
            {
                chunk.OnPlayerEnter += UpdateChunks;
            }
        }

        private void OnDisable()
        {
            foreach (var chunk in _chunks)
            {
                chunk.OnPlayerEnter -= UpdateChunks;
            }
        }

        private void UpdateChunks(Chunk currentChunk)
        {
            UpdateCurrentChunkIndex(currentChunk);
            _activeChunks.Clear();
            
            //Show Chunks
            if (_prevIndex >= 0)
            {
                _chunks[_prevIndex].gameObject.SetActive(true);
                _activeChunks.Add(_chunks[_prevIndex]);
                
            }
            _chunks[_currentIndex].gameObject.SetActive(true);
            _activeChunks.Add(_chunks[_currentIndex]);
            if (_subIndex < _chunks.Count)
            {
                _chunks[_subIndex].gameObject.SetActive(true);
                _activeChunks.Add(_chunks[_subIndex]);
            }
            
            //TODO:Hide Chunks
            // List<Chunk> hiddenChunks = _chunks.FindAll(chunk => _activeChunks.ForEach(chunk1 => chunk != chunk1))
            // foreach (var hiddenChunk in hiddenChunks)
            // {
            //     hiddenChunk.gameObject.SetActive(false);
            // }
        }

        private void UpdateCurrentChunkIndex(Chunk currentChunk)
        {
            _prevIndex = GetPrevChunkIndexOfAChunk(_chunks, currentChunk);
            _currentIndex = _chunks.FindIndex(current => current == currentChunk);
            _subIndex = GetSubChunkIndexOfAChunk(_chunks, currentChunk);
        }

        private int GetPrevChunkIndexOfAChunk(List<Chunk> array, Chunk child)
        {
            return array.FindIndex(index => index == child) - 1;
        }
        
        private int GetSubChunkIndexOfAChunk(List<Chunk> array, Chunk child)
        {
            return array.FindIndex(index => index == child) + 1;
        }
    }
}