# ThrowDice
<img src="http://berkpekatik.com/gif/zar.gif" title="how to use?">
<p>Download <a href="">here</a></p>
You can use global trigger events<br>
<h2>C#</h2>
TriggerEvent("Dice:Zar"); // rolling 2,12 dice<br/>
TriggerEvent("Dice:ZarAt",2,12); // rolling provided number<br/>
TriggerEvent("Dice:ZarListe"); // giving previous roll dice list<br/>
  
<h2>LUA</h2>
TriggerEvent("Dice:Zar") // rolling 2,12 dice<br/>
TriggerEvent("Dice:ZarAt",2,12) // rolling provided number<br/>
TriggerEvent("Dice:ZarListe") // giving previous roll dice list  <br/>
  
<h2>JS</h2>
emit("Dice:Zar") // rolling 2,12 dice<br/>
emit("Dice:ZarAt",2,12) // rolling provided number<br/>
emit("Dice:ZarListe") // giving previous roll dice list<br/>
  
or use with chat commands ( /zar, /zarliste)
