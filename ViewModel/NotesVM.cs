using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EverNoteCloneWPF.Model;
using EverNoteCloneWPF.ViewModel.Commands;
using EverNoteCloneWPF.ViewModel.Helpers;

namespace EverNoteCloneWPF.ViewModel
{
    public class NotesVM: ObservableObject
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }
        private Notebook _selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return _selectedNotebook; }
            set
            {
                _selectedNotebook = value;
                OnPropertyChanged(nameof(SelectedNotebook));
                GetNotes();
            }
        }

        public NotesVM()
        {
            NewNoteBookCommand = new DelegateCommand<Notebook>(CreateNewNoteBook);
            NewNoteCommand = new DelegateCommand<Notebook>(CreateNewNote, CanNewNoteExecuted);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();
            GetNoteBooks();
        }

        public ObservableCollection<Note> Notes { get; set; }
        public DelegateCommand<Notebook> NewNoteBookCommand { get; set; }
        public DelegateCommand<Notebook> NewNoteCommand { get; set; }
        private void CreateNewNote(Notebook selectedNotebook)
        {
            Note newNote = new Note()
            {
                NoteBookId = selectedNotebook.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = $"Note for {DateTime.Now.ToString(CultureInfo.InvariantCulture)}",
            };

            DatabaseHelper.Insert(newNote);

            GetNotes();
        }

        private void CreateNewNoteBook(Notebook obj)
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook",
            };
            DatabaseHelper.Insert(newNotebook);

            GetNoteBooks();
        }

        private bool CanNewNoteExecuted(Notebook selectedNotebook)
        {
            return selectedNotebook != null;
        }

        private void GetNoteBooks()
        {
            var notebooks = DatabaseHelper.Read<Notebook>();

            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = DatabaseHelper.Read<Note>().Where(n => n.NoteBookId == SelectedNotebook.Id);

                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }
    }
}
