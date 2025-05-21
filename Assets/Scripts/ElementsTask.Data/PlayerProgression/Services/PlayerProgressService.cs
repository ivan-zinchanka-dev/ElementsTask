using System;
using System.IO;
using System.Threading.Tasks;
using ElementsTask.Common.Data.Crypto.Abstractions;
using ElementsTask.Data.PlayerProgression.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace ElementsTask.Data.PlayerProgression.Services
{
    public class PlayerProgressService : IPlayerProgressService
    {
        private const string DataFolderName = "Data";
        private const string ProgressFileName = "progress.edm";
        
        private DirectoryInfo _dataDirectoryInfo;
        private PlayerProgress _playerProgress;
        
        private readonly IStreamCryptoService _cryptoService;
        
        public PlayerProgressService(IStreamCryptoService cryptoService)
        {
            _cryptoService = cryptoService;
            
            InitializeDataDirectory();
        }
        
        public async Task<PlayerProgress> GetPlayerProgressAsync()
        {
            if (_playerProgress == null)
            {
                await LoadPlayerProgressAsync();
            }
            
            return _playerProgress;
        }
        
        private void InitializeDataDirectory()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, DataFolderName);
            _dataDirectoryInfo = new DirectoryInfo(fullPath);
        
            if (!_dataDirectoryInfo.Exists)
            {
                _dataDirectoryInfo.Create();
            }
            
            Debug.Log("[PlayerProgressService] Target directory: " + _dataDirectoryInfo.FullName);
        }
        
        private string GetFullFileName()
        {
            return Path.Combine(_dataDirectoryInfo.FullName, ProgressFileName);
        }

        private async Task LoadPlayerProgressAsync()
        {
            string fullFileName = GetFullFileName();

            if (!File.Exists(fullFileName))
            {
                _playerProgress = new PlayerProgress();
                File.Create(fullFileName);
            }
            else
            {
                try
                {
                    string encryptedNotation = await File.ReadAllTextAsync(fullFileName);

                    if (string.IsNullOrEmpty(encryptedNotation) || string.IsNullOrWhiteSpace(encryptedNotation))
                    {
                        _playerProgress = new PlayerProgress();
                    }
                    else
                    {
                        string jsonNotation = _cryptoService.Decrypt(encryptedNotation);
                            
                        _playerProgress = JsonConvert.DeserializeObject<PlayerProgress>(jsonNotation);
                        Assert.IsNotNull(_playerProgress);
                    }
                }
                catch (Exception ex)
                {
                    _playerProgress = new PlayerProgress();
                    
                    var loggedException = 
                        new Exception("[ProgressDataAdapter] Deserialization of progress_model.edm is failed. Default model created");
                    Debug.LogException(loggedException);
                            
                    File.Create(fullFileName);
                }
                
            }

            _playerProgress.OnDemandSave += OnDemandSaveAsync;
        }
        
        private async void OnDemandSaveAsync()
        {
            await Task.Run(SaveProgressModel);
        }
        
        private void SaveProgressModel()
        {
            string jsonNotation = JsonConvert.SerializeObject(_playerProgress);
            string encryptedNotation = _cryptoService.Encrypt(jsonNotation);
                
            lock (_playerProgress)
            {
                File.WriteAllText(GetFullFileName(), encryptedNotation);
            }
        }

        public void ClearProgress()
        {
            File.Delete(GetFullFileName());
        }
        
        public void Dispose()
        {
            _playerProgress.OnDemandSave -= OnDemandSaveAsync;
        }
    }
}