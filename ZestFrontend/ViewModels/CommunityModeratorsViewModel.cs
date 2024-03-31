using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Community), "Community")]
	public partial class CommunityModeratorsViewModel : ObservableObject
    {
        CommunityService _communityService;
		AuthService _authService;
        public CommunityModeratorsViewModel(CommunityService communityService, AuthService authService)
        {
            this._communityService = communityService;
			this._authService = authService;
        }
        [ObservableProperty]
        CommunityDTO community;
		[ObservableProperty]
		bool isModerator;
		[ObservableProperty]
		string buttonText;
        public ObservableCollection<UserDTO> Moderators { get; private set; } = new ();
		public ObservableCollection<UserDTO> Candidates { get; private set; } = new();
		public async Task GetModerators()
        {
            Moderators.Clear();
            foreach (var item in await _communityService.GetModeratorsByCommunity(Community.Id))
            {
                Moderators.Add(item);
            }
        }
		public async Task GetCandidates()
		{
			Candidates.Clear();
			foreach (var item in await _communityService.GetModeratorCandidatesByCommunity(Community.Id))

			{
				Candidates.Add(item);
			}
		}
        [RelayCommand]
        async Task ApproveCandidateAsync(UserDTO account)
        {
            var result = await _communityService.ApproveCandidate(account.Id, Community.Id);
			if (result.IsSuccessStatusCode)
			{
				Candidates.Remove(account);
				Moderators.Add(account);
			}
        }
        [RelayCommand]
		async Task DisapproveCandidateAsync(UserDTO account)
		{
			var result = await _communityService.RemoveModerator(account.Id, Community.Id);
			if (result.IsSuccessStatusCode)
			{
				Candidates.Remove(account);
			}
				
			
		}
		[RelayCommand]
		async Task AddMeAsync(UserDTO account)
		{
			await _communityService.AddCommunityModerator(_authService.Id, Community.Id);
		}
		async partial void OnCommunityChanged(CommunityDTO value)
		{
            await GetModerators();
            await GetCandidates();
			var isMod  = await _communityService.IsModerator(_authService.Id, Community.Id);
			if (isMod)
			{
				IsModerator = true;
				ButtonText = "Remove me";
			}
			else
			{
				IsModerator = false;
				ButtonText = "Add me";
			}
		}
		[RelayCommand]
		async Task GoToUserDetailPageAsync(UserDTO user)
		{
			if (user == null) return;

			await Shell.Current.GoToAsync($"{nameof(UserDetailsPage)}?id={user.Id}", true,
				new Dictionary<string, object>
			{
			{"User", user }
			});
		}
		[RelayCommand]
		async Task ChangeModeratorshipStatusAsync()
		{
			if (IsModerator)
			{

				var result = await _communityService.RemoveModerator(_authService.Id,Community.Id);
				if (result.StatusCode == HttpStatusCode.OK)
				{
					ButtonText = "Add me";
					IsModerator = false;
				}
			}
			else
			{

				var result = await _communityService.AddCommunityModerator(_authService.Id, Community.Id);
				if (result.IsSuccessStatusCode)
				{
					ButtonText = "Remove me";
					IsModerator = true;
				}
			}
		}
	}
}
