## Projektplanung
### Aufgabe:
Erstellen eines Programms zur Unterstützung des Einkaufs.
Es soll dazu eine Oberfläche entwickelt werden und im Hintergrund eine Datenbank.
Ziel ist es dass in der Gui die Daten angezeigt werden und bei niedrigem Bestand benachrichtigt wird.
lololol  
### Architektur:
Datenbank: SQL  
Gui: WPF  
Programmiersprachen: C#  
Mockup: Drawio  
Versionsverwaltung: (Git)  

### Funktionen:
- Anzeigen des Materialsbestandes
- Livebestandsanalyse
	- Dashboard für Anzeigen von kritischen Beständen
- Suchfunktion für Artikel
- Artikelpflege
- Auslösen von Bestellungen


### Datenbanktabelle
Name: Artikelbestand
| Materialnummer (PK) | Materialnamen | Warengruppe | Menge | Grenzwert |
| ------------------- | ------------- | ----------- | ----- | --------- |
|                     |               |             |       |           |
