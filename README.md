# Magic Episode Sort

**For the love of god... if you wanna try this, don't run it on, like, your entire media library lmao Give it a try in a small folder with some sample downloads.**

### First of all, never download tv shows. That's bad, mkay?

But let's say you really, really wanted to download all 50 episodes that came out this week, and then you wanted to throw them onto your media library, PLEX/EMBY enabled NAS. What are you gonna do? Just chuck 'em on there all willy nilly??

**MagicEpisodeSort** searches through all directories in its current directory and pulls all **mp4, mkv, avi, webm** files. 
It then finds the season number (by matching the formats: **S00E00, S0E0, S00E0000, S00E00-00, S00E00-E00**) and the series name 
(assumes that the series name directly preceedes the season/episode number).

Once that's done, the program creates directories for each series and season number in a folder named "Magic-Sorted" (default, can be changed in config). For example, 
`{root}/A.Third.Show.S12E0910.WEB.x264/A.Third.Show.S12E0910.episode-name.WEB.x264.mp4` goes into `{root}/Magic-Sorted/A Third Show/Season 12/A.Third.Show.S12E0910.episode-name.WEB.x264.mp4`.

### Configuring the Output

After running the first time, **MagicEpisodeSort** creates a JSON config file in root where you can adjust the output of series names and the default directory name of 
the location of sorted episodes. For example: 

```
	{
		"SortedDirectory": "Sorted Episodes",
		"CustomSeriesNames":[
			["My Aunts First Show", "My Aunt's 1st Show"], 
			["Bettercasing Please", "BetterCasing Please"]
		]
	}
```

This will set the directory name as "Sorted Episodes", and will change any episodes with the series title "My Aunts First Show" to "My Aunt's 1st Show" and "Bettercasing Please" to "BetterCasing Please".
This is especially useful for series with apostrophes in the name (they usually don't have apostrophes in the file name), and series with names containing special casing (a series with the name "CodingBestPractices 101"
will be named "Codingbestpractices 101" by **MagicEpisodeSort**).

Every time you run **MagicEpisodeSort** it also checks if this is the first time you're sorting this series and, if it is, it automatically adds an entry into the config for ease of update.

### Most importantly though, I do not condone illegal downloading of tv shows. Don't do it.

### To Do:

 - [ ] Match files with the season/episode format of AxBB
 - [X] Configs for: forcing names of series, folder structure, etc.
 - [ ] Add UI for managing configs
 - [x] Config auto population of series names after every run, for ease of update
