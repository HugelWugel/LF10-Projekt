[GitHub Link](https://github.com/HugelWugel/LF10-Projekt/tree/main)
# Dokumentation
## Projektplanung
### Ist-Analyse
Es ist keine Software und Prozesse vorhanden
### Soll-Konzept
Das Unternehmen plant die Erstellung eines eigenen DBMS zur Verwaltung der Artikel in Ihrem Lager und zur Unterstützung der Abteilung Einkauf. Es soll in C# entwickelt werden und Verbindung zu SQL Servern möglich sein. Ebenso soll das Programm erweiterbar sein.
### Use-Case
![use-case](/Bilder/UseCase_final.png)
### Mockups
![Hauptseite](/Bilder/Dashboard.png)
![Bestandsliste](/Bilder/Bestandsliste.png)
![Popups](/Bilder/Popups.png)
## Durchführung
### Erstellen der Datenbank
Da die Daten zwischen Mitarbeitern gleich sein und vom Einkauf zugänglich sein soll, musste eine Datenbank erstellt werden.
![er-modell](/Bilder/ERModellLF10.png)

```SQL
CREATE TABLE Lagerbestand(
	Materialnummer int IDENTITY(1,1) PRIMARY KEY,
	Materialname varchar(255) NOT NULL,
	Warengruppe varchar (255),
	Menge int,
	Grenzwert int
);
```

### Entwickeln der Desktopanwendung
Da das Programm auf Windows-Computer laufen soll und eine Oberfläche benötigt wird, haben wir uns für eine WPF-Anwendung entschieden. Mit WPF hat man immer noch die Möglichkeit Änderungen am Aussehen vorzunehmen.
#### Erstellen der Oberfläche
In der Planung haben wir entschieden, dass die Hauptseite bzw. das Dashboard und die Liste mit dem gesamten Bestand, zwei separate Pages sein werden. Da wir beim Entwickeln herausgefunden haben, dass WPF ein eingebautes Element hat, was den zuvor geplanten Aufbau mit nur einer Page und zusätzlicher Navigation ermöglicht, haben wir uns für die Tabitems entschieden. Zwischen den beiden Hauptansichten kann dadurch am oberen Fensterrand, zwischen den beiden Ansichten gewechselt werden. Bei beiden Ansichten ist der Hauptbestandteil eine Tabelle.
Das Dashboard beinhaltet zusätzlich einen Button der später eine Funktion zum bestellen haben soll.
Die Bestandsliste beinhaltet neben der Tabelle drei Buttons, die die Funktionen haben Einträge zu löschen, bearbeiten und hinzuzufügen. Beim betätigen von Knöpfen erscheint ein Popupfenster welches Werte des ausgewählten Eintrags anzeigt oder die gewünschten Werte zum hinzufügen anzeigt.
#### Erstellen der Datenbankfunktionen
Da mit einer Datenbank gearbeitet wird und die Datagrids bei Laufzeit aktualisiert werden sollen, mussten wir zwei Observable Collections erstellen. Ebenso musste zu der Datenbank die Verbindung definiert werden. 
```C#
public class dbService
    {
        private static string databasePath = @"C:\Users\simmatm\source\repos\LF10\Lager.mdf";
        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True";
        public static DataTable Bestand = new DataTable();
        public static DataTable kritBes = new DataTable();
        public static ObservableCollection<Lagerartikel> AllArtikel = new ObservableCollection<Lagerartikel>();
        private static ObservableCollection<kritArtikel> KritArtikel = new ObservableCollection<kritArtikel>();
```
Es werden zwei Queries benötigt um einmal die gesamte Bestandsliste abzufragen und einmal nur Materialien wo die Menge unter dem eingetragenem Grenzwert liegt (kritischer Bestand)
```C#
public ObservableCollection<Lagerartikel> getAllData()
{
	#Leeren der Einträge um doppelung zu Vermeiden
    AllArtikel.Clear();
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
	    #Abfrage zum selektieren aller Einträge
        string query = "SELECT * FROM Lagerbestand where Materialnummer is not null";
        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                AllArtikel.Add(new Lagerartikel
                {
                    Materialnummer = Convert.ToInt32(reader["Materialnummer"]),
                    Materialname = reader["Materialname"].ToString(),
                    Warengruppe= reader["Warengruppe"].ToString(),
                    Menge = Convert.ToInt32(reader["Menge"]),
                    Grenzwert = Convert.ToInt32(reader["Grenzwert"])
                });
            }
        }
        connection.Close();
    }
    return AllArtikel;
}
```

```C#
public ObservableCollection<kritArtikel> getKritData()
{
	#Leeren der Einträge um doppelung zu Vermeiden
    KritArtikel.Clear();
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        #Abfrage zum selektieren der kritischen Einträge
        string query = "Select Materialnummer, Menge, Grenzwert From Lagerbestand Where Menge < Grenzwert and Materialnummer is not null";
        SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                KritArtikel.Add(new kritArtikel
                {
                    Materialnummer = Convert.ToInt32(reader["Materialnummer"]),
                    Menge = Convert.ToInt32(reader["Menge"]),
                    Grenzwert = Convert.ToInt32(reader["Grenzwert"])
                });
            }
        }
        connection.Close();
    }
    return KritArtikel;
}
```
Ebenso wird eine Funktion zum Bearbeiten, wie Löschen von Einträgen benötigt.
```C#
public static void changeDataEntry(int id, string materialname, string warengruppe, int menge, int grenzwert)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = $"Update Lagerbestand SET Materialname = @Materialname, Warengruppe = @Warengruppe, Menge = @Menge, Grenzwert = @Grenzwert WHERE Materialnummer = @ID";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            connection.Open();
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@Materialname", materialname);
            command.Parameters.AddWithValue("@Warengruppe", warengruppe);
            command.Parameters.AddWithValue("@Menge", menge);
            command.Parameters.AddWithValue("@Grenzwert", grenzwert);

            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
```

```C#
public static int deleteDataEntry(List<int> ids)
{
    int rowsAffected;
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = $"DELETE FROM Lagerbestand WHERE Materialnummer IN ({string.Join(",", ids.Select((id, index) => $"@id{index}"))})";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            connection.Open();
            for (int i = 0; i < ids.Count; i++)
            {
                command.Parameters.AddWithValue($"@id{i}", ids[i]);
            }
            rowsAffected = command.ExecuteNonQuery();
        }
        connection.Close();
    }
    return rowsAffected;
}
```

#### Entwickeln des Mainwindows
Es gibt 2 verschiedene C#-Dateien. Einmal das Mainwindow, wo die beiden Hauptansichten sind, und einmal das Popup. Der Bestellbutton hat in diesem Projekt keine Funktion bekommen und wird in weiteren Projekten ergänzt. Der Löschen, bearbeiten und hinzufügen-Button, auf der zweiten Ansicht bekommen eine Funktion. Der löschen und bearbeiten Button, sind standardmäßig deaktiviert und werden erste aktiviert wenn ein Eintrag aus dem Datagrid selektiert ist. Sind mehr als 1 Eintrag selektiert, dann wird der bearbeiten Button wieder deaktiviert und nur löschen ist möglich. 
```c#
private void AllDataTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (AllDataTable.SelectedCells != null)
    {
        if (AllDataTable.SelectedItems.Count == 1)
        {
            bearbeitenButton.IsEnabled = true;
            loeschenButton.IsEnabled = true;
        }
        else if (AllDataTable.SelectedItems.Count > 1)
        {
            loeschenButton.IsEnabled = true;
            bearbeitenButton.IsEnabled = false;
        }
        else
        {
            loeschenButton.IsEnabled = false;
            bearbeitenButton.IsEnabled = false;
        }
    }
    else
    {
        loeschenButton.IsEnabled = false;
        bearbeitenButton.IsEnabled = false;
    }
}
```
Beim betätigen des löschen Buttons, wird eine Messagebox in den Vordergrund geschoben, wo der Nutzer sieht wie viele Zeilen selektiert sind und bestätigen soll, diese zu löschen. Beim drücken von ok erhält der Nutzer noch eine Benachrichtigung.
```C#
private void loeschenButton_Click(object sender, RoutedEventArgs e)
{
    MessageBoxResult result = MessageBox.Show($"Möchten Sie alle {AllDataTable.SelectedItems.Count} selektieren Einträge löschen?", "Fortfahren", MessageBoxButton.OKCancel, MessageBoxImage.Hand);
    if (result == MessageBoxResult.OK)
    {
        List<int> ids = new List<int>();
        for (int i = 0; AllDataTable.SelectedItems.Count > i; i++)
        {
            Lagerartikel ausgewählterArtikel = AllDataTable.SelectedItems[i] as Lagerartikel;
            ids.Add(ausgewählterArtikel.Materialnummer);
        }
        int affectedRows = dbService.deleteDataEntry(ids);
        MessageBox.Show($"{affectedRows} erfolgreich gelöscht", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
        LoadData();
    }
}
```
Die Buttons bearbeiten und hinzufügen lösen beide beim betätigen, die Methode showPopup aus. Dort ist ein switch-case eingebaut das Abhängig welcher Button gedrückt wurde, die Textboxen des Popups leert oder mit Werten des selektierten Eintrags befüllt. 
```C#
public void openPopup(int action)
{
    switch (action)
    {
        case 1:
            popup.clearTextboxes();
            popup.Show();
            popup.Activate();
            popup.PopupActionName.Content = "Eintrag hinzufügen";
            break;
        case 2:
            popup.PopupActionName.Content = "Eintrag bearbeiten";
            Lagerartikel ausgewählterArtikel = AllDataTable.SelectedItem as Lagerartikel;
            popup.updateTextboxes(ausgewählterArtikel);
            popup.Show();
            popup.Activate();
            break;
    }
}
```
#### Entwickeln des Popups
Das Popups enthält vier Funktionen. Zwei Funktionen für die Buttons ok und cancel und zwei für das manipulieren der Textboxen. Die Manipulation der Textboxen erfolgt über updateTextboxes und clearTextboxes. Bei clearTextboxes werden die Textboxen mit leeren Strings gefüllt. Bei updateTextboxes werden diese mit den Werten des ausgewählten Eintrags aus dem Mainwindow ergänzt. Ebenso wird jeweils ein ID Feld sichtbar oder versteckt, um den Nutzer sichtbar zu machen welchen Eintrag er auch ausgewählt hatte.
```C#
public void updateTextboxes(Lagerartikel Eintrag)
{
    idTextbox.Text = Eintrag.Materialnummer.ToString();
    NameTextbox.Text = Eintrag.Materialname;
    WarengruppeTextbox.Text = Eintrag.Warengruppe;
    MengeTextbox.Text = Eintrag.Menge.ToString();
    GrenzwertTextbox.Text = Eintrag.Grenzwert.ToString();

    idLabel.Visibility = Visibility.Visible;
    idTextbox.Visibility = Visibility.Visible;
}

public void clearTextboxes()
{
    idTextbox.Text = "";
    NameTextbox.Text = "";
    WarengruppeTextbox.Text = "";
    MengeTextbox.Text = "";
    GrenzwertTextbox.Text = "";

    idLabel.Visibility = Visibility.Hidden;
    idTextbox.Visibility = Visibility.Hidden;
}
```
Der cancel Button löst die Funktion cancelButton_Click aus, welche das Popup Fenster einfach nur versteckt und so dem Nutzer alles andere wieder freischaltet.
Der ok Button löst die okButton_Click funktion aus, die wieder ein switch-case hat. Dort wird abhängig welcher Button beim Mainwindow gedrückt wurde, entweder createDateEntry oder changeDataEntry aus.
```C#
private void okButton_Click(object sender, RoutedEventArgs e)
{
    switch (PopupActionName.Content)
    {
        case "Eintrag hinzufügen":
            dbService.createDataEntry(NameTextbox.Text, WarengruppeTextbox.Text, Convert.ToInt32(MengeTextbox.Text), Convert.ToInt32(GrenzwertTextbox.Text));
            MessageBox.Show($"Eintrag erstellt", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            mainWindow.LoadData();
            Hide();
            break;
        case "Eintrag bearbeiten":
            dbService.changeDataEntry(Convert.ToInt32(idTextbox.Text), NameTextbox.Text, WarengruppeTextbox.Text, Convert.ToInt32(MengeTextbox.Text), Convert.ToInt32(GrenzwertTextbox.Text));
            MessageBox.Show($"Eintrag bearbeitet", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            mainWindow.LoadData();
            Hide();
            break;
    }
}
```
