# Magic Episode Sort

**For the love of god... if you wanna try this, don't run it on, like, your entire media library lmao Give it a try in a small folder with some sample downloads.**

### First of all, never download tv shows. That's bad, mkay?

But let's say you really, really wanted to download all 50 episodes that came out this week, and then you wanted to throw them onto your media library, PLEX/EMBY enabled NAS. What are you gonna do? Just chuck 'em on there all willy nilly??

**No!** *Of course not.*

You gotta create folders for the shows and then folders for the seasons and then move the video files and leave all the garbage behind. Ain't nobody got time for that.

This tiny software will solve the problem of cluttered hard drives all around the world. Throw the software into your favorite downloads folder, run it, and it'll sort everything all by itself.

You just need delete what's left behind yourself.

### How it Works

**MagicEpisodeSort** searches through all directories in its current directory and pulls all **mp4, mkv, avi, webm** files. 
It then finds the season number (by matching the formats: **S00E00, S0E0, S00E0000, S00E00-00, S00E00-E00**) and the series name (assumes that the series name directly preceedes the season/episode number).

Once that's done, the program creates directories for each series and season number in a folder named "Magic-Sorted". For example, 
`{root}/A.Third.Show.S12E0910.WEB.x264/A.Third.Show.S12E0910.episode-name.WEB.x264.mp4` goes into `{root}/A Third Show/Season 12/A.Third.Show.S12E0910.episode-name.WEB.x264.mp4`.

### To Do:

- Match files with the season/episode format of AxBB
- Configs for: forcing names of series, folder structure, etc.

### Most importantly though, I do not condone illegal downloading of tv shows.
