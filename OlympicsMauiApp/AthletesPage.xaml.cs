using System;
using System.Linq;
using Microsoft.Maui.Controls;
using SQLite;

namespace OlympicsMauiApp
{
    public partial class AthletesPage : ContentPage
    {
        public AthletesPage()
        {
            InitializeComponent();

            // Populate countriesListView and sportsListView with data
            countriesListView.ItemsSource = DB.conn.Table<Participant2016Summer>()
                                                   .Select(p => p.Country)
                                                   .Distinct()
                                                   .OrderBy(c => c)
                                                   .ToList();

            sportsListView.ItemsSource = DB.conn.Table<Participant2016Summer>()
                                                 .Select(p => p.Sport)
                                                 .Distinct()
                                                 .OrderBy(s => s)
                                                 .ToList();
        }

        private void OnCountrySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null && sportsListView.SelectedItem != null)
            {
                string selectedCountry = e.SelectedItem.ToString();
                string selectedSport = sportsListView.SelectedItem.ToString();
                string selectedEvent = eventsListView.SelectedItem?.ToString();

                if (selectedEvent != null)
                {
                    // Update both events and athletes lists
                    UpdateEventsList(selectedCountry, selectedSport);
                    UpdateAthletesList(selectedCountry, selectedSport, selectedEvent);
                }
                else
                {
                    // Update only events list as no event is selected yet
                    UpdateEventsList(selectedCountry, selectedSport);
                }
            }
        }

        private void UpdateEventsList(string country, string sport)
        {
            eventsListView.ItemsSource = DB.conn.Table<Participant2016Summer>()
                                        .Where(p => p.Country == country && p.Sport == sport)
                                        .Select(p => p.Event)
                                        .Distinct()
                                        .OrderBy(ev => ev)
                                        .ToList();
        }

        private void UpdateAthletesList(string country, string sport, string eventStr)
        {
            athletesListView.ItemsSource = DB.conn.Table<Participant2016Summer>()
                                          .Where(p => p.Country == country && p.Sport == sport && p.Event == eventStr)
                                          .OrderByDescending(p => p.Medal)
                                          .ToList();
        }

        private void OnSportSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null && countriesListView.SelectedItem != null)
            {
                string selectedSport = e.SelectedItem.ToString();
                string selectedCountry = countriesListView.SelectedItem.ToString();

                UpdateEventsList(selectedCountry, selectedSport);
            }
        }

        private void OnEventSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null && countriesListView.SelectedItem != null && sportsListView.SelectedItem != null)
            {
                string selectedEvent = e.SelectedItem.ToString();
                string selectedCountry = countriesListView.SelectedItem.ToString();
                string selectedSport = sportsListView.SelectedItem.ToString();

                UpdateAthletesList(selectedCountry, selectedSport, selectedEvent);
            }
        }

        private async void OnAthleteSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Participant2016Summer selectedAthlete)
            {
                var athleteEvents = DB.conn.Table<Participant2016Summer>()
                                           .Where(p => p.Name == selectedAthlete.Name)
                                           .Select(p => p.Event)
                                           .ToList();

                string athleteDetails = string.Join(", ", athleteEvents);

                await DisplayAlert(selectedAthlete.Name, athleteDetails, "Close");
            }
        }

    }
}