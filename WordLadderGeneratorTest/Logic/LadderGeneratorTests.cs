using Synergenic.Windows.CLI.WordLadderGenerator.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Synergenic.Windows.CLI.WordLadderGenerator.Logic.Tests
{
    /// <summary>
    /// Define a Set of Unit Tests for the Ladder Generator Class
    /// </summary>
    public class LadderGeneratorTests
    {
        private readonly ITestOutputHelper output;

        /// <summary>
        /// Overload the Constructor with a constructor that accepts a ITestOutputHelper so that the framework
        /// can inject an Output Helper object allowing us to log detail to the Unit Test console.
        /// </summary>
        /// <param name="testOutputHelper">An ITestOutputHelper Output Helper Object</param>
        public LadderGeneratorTests(ITestOutputHelper testOutputHelper)
        {
            output = testOutputHelper;
        }


        /// <summary>
        /// Test to ensure that the Ladder Generator handles the case where an Empty Dictionary
        /// is passed and produces no output (as the resultant data-structures are empty)
        /// </summary>
        [Fact]
        public void CheckEmptyDictionary()
        {
            // Arrange
            Logger log = new Logger();
            LadderGenerator generator = new LadderGenerator(log);
            IEnumerable<IList<string>> wordLadders = new List<List<string>>();
            List<string> dictionary = new List<string>();

            // Act
            wordLadders = generator.GetWordLadders("spin", "spot", dictionary);
            OutputDetails(dictionary, wordLadders);

            // Assert
            Assert.Empty(wordLadders);
        }

        /// <summary>
        /// Test to ensure that the Ladder Generator handles the case where an invalid or
        /// empty Start or End Word is passed and produces no output (as the resultant 
        /// data-structures are empty)
        /// </summary>
        [InlineData("spin", "")]        // End Word Empty
        [InlineData("", "spot")]        // Start Word Empty
        [InlineData("", "")]            // Both Words Empty
        [InlineData("pppp", "spot")]    // Start Word Not In Dictionary
        [InlineData("spin", "pppp")]    // End Word Not In Dictionary
        [InlineData("zzzz", "zzzz")]    // Neither Word in Dictionary
        [Theory]
        public void CheckStartAndEndWords(string startWord, string endWord)
        {
            // Arrange
            Logger log = new Logger();
            LadderGenerator generator = new LadderGenerator(log);
            IEnumerable<IList<string>> wordLadders = new List<List<string>>();
            List<string> dictionary = GenerateTestDictionary();

            // Act
            wordLadders = generator.GetWordLadders(startWord, endWord, dictionary);
            OutputDetails(dictionary, wordLadders);

            // Assert
            Assert.Empty(wordLadders);
        }

        /// <summary>
        /// Test to ensure the Ladder Generator produces the correct Word Ladder for the given
        /// Start and End Words. (Here we are using the sample data from the Blue Prism documentation.
        /// </summary>
        [Fact]
        public void CheckWordLadderCorrectWithDemoFromBluePrismEmail()
        {
            // Arrange
            Logger log = new Logger();
            List<IList<string>> wordLadders;
            List<string> dictionary = GenerateTestDictionary();
            LadderGenerator generator = new LadderGenerator(log);

            // Act
            wordLadders = generator.GetWordLadders("spin", "spot", dictionary).ToList();
            OutputDetails(dictionary, wordLadders);

            // Assert
            Assert.Single(wordLadders);
            Assert.Equal("spin", wordLadders[0][0]);
            Assert.Equal("spit", wordLadders[0][1]);
            Assert.Equal("spot", wordLadders[0][2]);
        }


        /// <summary>
        /// Test to ensure the Ladder Generator produces the correct Word Ladder for the given
        /// Start and End Words. Here we are looking for a Ladder using the start word 'four' and
        /// the end word 'tent'. A single ladder of six words is expected.
        /// </summary>
        [Fact]
        public void CheckWordLadderCorrect2()
        {
            // Arrange
            Logger log = new Logger();
            List<IList<string>> wordLadders;
            List<string> dictionary = GenerateTestDictionary();
            LadderGenerator generator = new LadderGenerator(log);

            // Act
            wordLadders = generator.GetWordLadders("four", "tent", dictionary).ToList();
            OutputDetails(dictionary, wordLadders);

            // Assert
            Assert.Single(wordLadders);
            Assert.Equal("four", wordLadders[0][0]);
            Assert.Equal("pour", wordLadders[0][1]);
            Assert.Equal("pout", wordLadders[0][2]);
            Assert.Equal("pont", wordLadders[0][3]);
            Assert.Equal("pent", wordLadders[0][4]);
            Assert.Equal("tent", wordLadders[0][5]);
        }

        /// <summary>
        /// Helper Method to Output details of the result to the Test Output to aid in analysis
        /// </summary>
        /// <param name="dictionary">The Word Dictionary used in the Test</param>
        /// <param name="wordLadders">The Word Ladders output by the Test</param>
        private void OutputDetails(List<string> dictionary, IEnumerable<IList<string>> wordLadders)
{
        output.WriteLine($"{dictionary.Count()} words were found in the Test Dictionary File.");

        int currentLadder = 1;
        wordLadders.ToList().ForEach(ladder =>
        {
            output.WriteLine($"Ladder {currentLadder} Found :");
            ladder.ToList().ForEach(w => output.WriteLine($"\t{w}"));
            currentLadder++;
        });
    }
        
        /// <summary>
        /// Helper Method to create a Test Word Dictionary.
        /// </summary>
        /// <returns>A Test Dictionary</returns>
        private List<string> GenerateTestDictionary()
        {
            return new List<string>()
            {
                "aaas", "ansi", "arpa", "astm", "abel", "acts", "adam", "aden", "agee", "aida", "ainu", "ajax", "alan", "alec", "alex", 
                "alps", "alva", 
                "ames", "amos", "andy", "anna", "anne", "arab", "ares", "asia", "avis", "aviv", "avon", "bema", "bstj", "bach", "baja", 
                "baku", "bali", "barr", "bart", "bela", "benz", "bern", "bert", "bess", "bini", "blum", "blvd", "boca", "bohr", "bois", 
                "bonn", "borg", "bose", "boyd", "brie", "bryn", "budd", "burr", "burt", "byrd", "cacm", "catv", "ccny", "cern", "cuny", 
                "cady", "cain", "carl", "carr", "celt", "chad", "chao", "chen", "chou", "clio", "cluj", "cobb", "cody", "cohn", "cole", 
                "corp", "cruz", "cuba", "dada", "dade", "dahl", "daly", "dana", "dane", "dave", "davy", "dido", "dodd", "doge", "dora", 
                "doug", "duma", "dunn", "dyke", "eeoc", "erda", "eben", "eden", "edna", "egan", "eire", "elba", "ella", "emil", "emma", 
                "enid", "enos", "eric", "erie", "erik", "eros", "ezra", "fifo", "fiji", "finn", "fisk", "foss", "fran", "frau", "fred", 
                "frey", "frye", "fuji", "gail", "galt", "gary", "gaul", "gina", "ginn", "gino", "gobi", "goff", "gogh", "greg", "guam", 
                "gwen", "gwyn", "haag", "haas", "hahn", "hans", "hays", "hebe", "hera", "herr", "hess", "hoff", "holm", "hopi", "howe", 
                "hoyt", "hugh", "hugo", "hurd", "hyde", "ieee", "ifni", "igor", "inca", "indo", "iowa", "iran", "iraq", "irma", "isis", 
                "ivan", "jacm", "jake", "jane", "java", "jeff", "jill", "joan", "joel", "john", "jose", "jove", "juan", "judd", "jude", 
                "judy", "july", "june", "jung", "juno", "jura", "kahn", "kane", "kant", "karl", "karp", "kate", "katz", "kemp", "kent", 
                "kerr", "kiev", "klan", "klux", "knox", "koch", "kong", "kuhn", "kurd", "kurt", "kyle", "lifo", "lana", "lang", "laos", 
                "lars", "laue", "lear", "lena", "leon", "levi", "lila", "lima", "lind", "lisa", "lise", "loeb", "lois", "loki", "lola", 
                "lomb", "lome", "lowe", "lucy", "luis", "luke", "lund", "lura", "lutz", "lyle", "lynn", "lyon", "lyra", "mach", "mali", 
                "mann", "mans", "marc", "mars", "marx", "mary", "mawr", "maya", "mayo", "mimi", "mira", "moen", "mohr", "moll", "mona", 
                "mont", "mudd", "muir", "muzo", "myra", "nasa", "nato", "ncaa", "ncar", "nimh", "noaa", "ntis", "nagy", "nair", "nash", 
                "nate", "nazi", "neal", "neff", "neil", "nell", "nero", "ness", "neva", "nile", "nina", "noah", "noel", "noll", "nora", 
                "opec", "osha", "odin", "ohio", "okay", "olaf", "olav", "olga", "olin", "oman", "opel", "orca", "orin", "oslo", "otis", 
                "otto", "ovid", "palo", "parr", "paso", "paul", "penh", "penn", "peru", "pete", "phil", "pict", "pitt", "pius", "polk", 
                "prof", "pugh", "pyle", "rotc", "rsvp", "ramo", "rand", "raul", "reid", "rena", "rene", "reub", "rhea", "rica", "rico", 
                "riga", "ritz", "rome", "rosa", "ross", "roth", "rowe", "rube", "rudy", "russ", "ruth", "ryan", "siam", "suny", "salk", 
                "sana", "sara", "saud", "saul", "scot", "sean", "seth", "shea", "sian", "sikh", "sims", "sino", "skye", "slav", "sony",
                "stan", "styx", "suez", "taft", "taos", "tass", "tess", "thai", "thea", "thor", "tina", "tito", "toby", "todd", "togo",
                "toni", "truk", "ucla", "usaf", "usda", "usgs", "usia", "usps", "ussr", "ulan", "unix", "urdu", "uris", "ursa", "utah",
                "vail", "veda", "vega", "vera", "vida", "viet", "vito", "voss", "weco", "waco", "wahl", "walt", "wang", "webb", "wehr",
                "wier", "witt", "wong", "ymca", "ywca", "yale", "york", "yost", "yuki", "yves", "zeus", "zion", "zorn", "zulu", "abbe",
                "abed", "abet", "able", "abut", "ache", "acid", "acme", "acre", "afar", "afro", "agar", "agog", "ague", "ahem", "ahoy",
                "aide", "aile", "airy", "ajar", "akin", "alai", "alan", "alba", "alga", "alia", "ally", "alma", "aloe", "also", "alto",
                "alum", "amen", "amid", "ammo", "amok", "amra", "anew", "ante", "anti", "anus", "apex", "apse", "aqua", "arch", "area",
                "argo", "aria", "arid", "army", "arty", "arum", "aryl", "ashy", "atom", "atop", "aunt", "aura", "auto", "aver", "avid",
                "avow", "away", "awry", "axes", "axis", "axle", "axon", "babe", "baby", "back", "bade", "bail", "bait", "bake", "bald",
                "bale", "balk", "ball", "balm", "band", "bane", "bang", "bank", "barb", "bard", "bare", "bark", "barn", "base", "bash",
                "bask", "bass", "bate", "bath", "batt", "baud", "bawd", "bawl", "bead", "beak", "beam", "bean", "bear", "beat", "beau",
                "beck", "beef", "been", "beep", "beer", "beet", "bell", "belt", "bend", "bent", "berg", "best", "beta", "beth", "bevy",
                "bhoy", "bias", "bibb", "bide", "bien", "bike", "bile", "bilk", "bill", "bind", "bing", "bird", "bite", "bitt", "blab",
                "blat", "bled", "blew", "blip", "blob", "bloc", "blot", "blow", "blue", "blur", "boar", "boat", "bock", "bode", "body",
                "bogy", "boil", "bold", "bole", "bolo", "bolt", "bomb", "bona", "bond", "bone", "bong", "bony", "book", "boom", "boon",
                "boor", "boot", "bore", "born", "boss", "both", "bout", "bowl", "boxy", "brad", "brae", "brag", "bran", "bray", "bred",
                "brew", "brig", "brim", "brow", "buck", "buff", "bulb", "bulk", "bull", "bump", "bunk", "bunt", "buoy", "burg", "burl",
                "burn", "burp", "bury", "bush", "buss", "bust", "busy", "butt", "buzz", "byte", "cafe", "cage", "cake", "calf", "call",
                "calm", "came", "camp", "cane", "cant", "cape", "capo", "card", "care", "carp", "cart", "case", "cash", "cask", "cast",
                "cave", "cede", "ceil", "cell", "cent", "chap", "char", "chat", "chaw", "chef", "chew", "chic", "chin", "chip", "chit",
                "chop", "chow", "chub", "chug", "chum", "cite", "city", "clad", "clam", "clan", "clap", "claw", "clay", "clef", "clip",
                "clod", "clog", "clot", "cloy", "club", "clue", "coal", "coat", "coax", "coca", "cock", "coco", "coda", "code", "coed",
                "coil", "coin", "coke", "cola", "cold", "colt", "coma", "comb", "come", "cone", "conn", "cony", "cook", "cool", "coon",
                "coop", "coot", "cope", "copy", "cord", "core", "cork", "corn", "cosh", "cost", "cosy", "coup", "cove", "cowl", "cozy",
                "crab", "crag", "cram", "crap", "craw", "crew", "crib", "crop", "crow", "crud", "crux", "cube", "cuff", "cull", "cult",
                "curb", "curd", "cure", "curl", "curt", "cusp", "cute", "cyst", "czar", "dais", "dale", "dame", "damn", "damp", "dang",
                "dank", "dare", "dark", "darn", "dart", "dash", "data", "date", "daub", "dawn", "daze", "dead", "deaf", "deal", "dean",
                "dear", "debt", "deck", "deed", "deem", "deep", "deer", "deft", "defy", "deja", "dell", "demi", "demo", "dent", "deny",
                "desk", "deus", "dewy", "dial", "dice", "dick", "died", "diem", "diet", "dill", "dime", "dine", "ding", "dint", "dire",
                "dirt", "disc", "dish", "disk", "diva", "dive", "dock", "dodo", "doff", "dole", "doll", "dolt", "dome", "done", "doom",
                "door", "dope", "dose", "dote", "dour", "dove", "down", "doze", "drab", "drag", "dram", "draw", "dreg", "drew", "drib",
                "drip", "drop", "drub", "drug", "drum", "dual", "duck", "duct", "duel", "duet", "duff", "duke", "dull", "duly", "dumb",
                "dump", "dune", "dung", "dunk", "dupe", "dusk", "dust", "duty", "dyad", "dyer", "dyne", "each", "earl", "earn", "ease",
                "east", "easy", "eave", "echo", "eddy", "edge", "edgy", "edit", "elan", "else", "emit", "emma", "enol", "envy", "epic",
                "etch", "even", "evil", "exam", "exec", "exit", "eyed", "face", "fact", "fade", "fail", "fain", "fair", "fake", "fall",
                "fame", "fang", "fare", "farm", "faro", "fast", "fate", "faun", "fawn", "faze", "fear", "feat", "feed", "feel", "feet", 
                "fell", "felt", "fend", "fern", "fest", "fete", "feud", "fiat", "fide", "fief", "fife", "file", "fill", "film", "find", 
                "fine", "fink", "fire", "firm", "fish", "fist", "five", "flag", "flak", "flam", "flan", "flap", "flat", "flaw", "flax", 
                "flea", "fled", "flee", "flew", "flex", "flip", "flit", "floc", "floe", "flog", "flop", "flow", "flub", "flue", "flux", 
                "foal", "foam", "foci", "fogy", "foil", "fold", "folk", "fond", "font", "food", "fool", "foot", "ford", "fore", "fork", 
                "form", "fort", "foul", "four", "fowl", "foxy", "fray", "free", "fret", "frog", "from", "fuel", "full", "fume", "fund", 
                "funk", "furl", "fury", "fuse", "fuss", "fuzz", "gaff", "gage", "gain", "gait", "gala", "gale", "gall", "game", "gang", 
                "gape", "garb", "gash", "gasp", "gate", "gaur", "gave", "gawk", "gaze", "gear", "geld", "gene", "gent", "germ", "gibe", 
                "gift", "gila", "gild", "gill", "gilt", "gird", "girl", "girt", "gist", "give", "glad", "glee", "glen", "glib", "glob", 
                "glom", "glow", "glue", "glum", "glut", "gnat", "gnaw", "goad", "goal", "goat", "goer", "goes", "gogo", "gold", "golf", 
                "gone", "gong", "good", "goof", "gore", "gory", "gosh", "gout", "gown", "grab", "grad", "gray", "grep", "grew", "grey", 
                "grid", "grim", "grin", "grip", "grit", "grow", "grub", "gulf", "gull", "gulp", "gunk", "guru", "gush", "gust", "gyro", 
                "hack", "hail", "hair", "hale", "half", "hall", "halo", "halt", "hand", "hang", "hank", "hard", "hare", "hark", "harm", 
                "harp", "hart", "hash", "hasp", "hast", "hate", "hath", "haul", "have", "hawk", "haze", "hazy", "head", "heal", "heap", 
                "hear", "heat", "heck", "heed", "heel", "heft", "heir", "held", "hell", "helm", "help", "hemp", "herb", "herd", "here", 
                "hero", "hewn", "hick", "hide", "high", "hike", "hill", "hilt", "hind", "hint", "hire", "hiss", "hive", "hoar", "hoax", 
                "hobo", "hock", "hold", "hole", "holt", "home", "homo", "hone", "hong", "honk", "hood", "hoof", "hook", "hoop", "hoot", 
                "hope", "horn", "hose", "host", "hour", "hove", "howl", "huck", "hued", "huff", "huge", "hulk", "hull", "hump", "hung", 
                "hunk", "hunt", "hurl", "hurt", "hush", "hymn", "ibex", "ibid", "ibis", "icky", "icon", "idea", "idle", "idly", "idol", 
                "iffy", "inch", "indy", "info", "into", "iota", "ipso", "iris", "iron", "isle", "itch", "item", "jack", "jade", "jail", 
                "jake", "java", "jazz", "jean", "jeep", "jerk", "jess", "jest", "jibe", "jilt", "jinx", "jive", "jock", "joey", "join", 
                "joke", "jolt", "joss", "jowl", "judo", "juju", "juke", "jump", "junk", "jure", "jury", "just", "jute", "kale", "kava", 
                "kayo", "keel", "keen", "keep", "kelp", "keno", "kept", "kern", "keto", "keys", "khan", "kick", "kill", "kilo", "kind", 
                "king", "kink", "kirk", "kiss", "kite", "kiva", "kivu", "kiwi", "knee", "knew", "knit", "knob", "knot", "know", "kola", 
                "kudo", "lace", "lack", "lacy", "lady", "laid", "lain", "lair", "lake", "lakh", "lama", "lamb", "lame", "lamp", "land", 
                "lane", "lank", "lard", "lark", "lase", "lash", "lass", "last", "late", "lath", "laud", "lava", "lawn", "laze", "lazy", 
                "lead", "leaf", "leak", "lean", "leap", "leek", "leer", "left", "lend", "lens", "lent", "less", "lest", "levy", "lewd", 
                "liar", "lice", "lick", "lied", "lien", "lieu", "life", "lift", "like", "lilt", "lily", "limb", "lime", "limp", "line", 
                "link", "lint", "lion", "lisp", "list", "live", "load", "loaf", "loam", "loan", "lobe", "lobo", "loch", "loci", "lock", 
                "loft", "loge", "logo", "loin", "loll", "lone", "long", "look", "loom", "loon", "loop", "loot", "lope", "lord", "lore", 
                "lose", "loss", "lost", "loud", "love", "luck", "lucy", "luge", "luke", "lull", "lulu", "lump", "lung", "lure", "lurk", 
                "lush", "lust", "lute", "luxe", "lynx", "lyre", "mace", "mack", "made", "magi", "maid", "mail", "maim", "main", "make", 
                "male", "mall", "malt", "mana", "mane", "many", "mare", "mark", "mart", "mash", "mask", "mass", "mast", "mate", "math", 
                "maul", "maze", "mead", "meal", "mean", "meat", "meek", "meet", "meld", "melt", "memo", "mend", "menu", "meow", "mere", 
                "mesa", "mesh", "mess", "mete", "mica", "mice", "mien", "miff", "mike", "mila", "mild", "mile", "milk", "mill", "milt", 
                "mind", "mine", "mini", "mink", "mint", "mire", "miss", "mist", "mite", "mitt", "moan", "moat", "mock", "mode", "mold", 
                "mole", "molt", "monk", "mood", "moon", "moor", "moot", "more", "morn", "mort", "moss", "most", "moth", "move", "much", 
                "muck", "muff", "mule", "mull", "mung", "muon", "murk", "muse", "mush", "musk", "must", "mute", "mutt", "myel", "myth", 
                "nail", "name", "nape", "nary", "nave", "navy", "neap", "near", "neat", "neck", "need", "neon", "nest", "neve", "newt", 
                "next", "nibs", "nice", "nick", "nigh", "nine", "node", "nolo", "none", "nook", "noon", "norm", "nose", "nosy", "note", 
                "noun", "nova", "novo", "nude", "null", "numb", "oath", "obey", "oboe", "ogle", "ogre", "oily", "oint", "okay", "okra", 
                "oldy", "omen", "omit", "once", "only", "onto", "onus", "onyx", "ooze", "opal", "open", "opus", "oral", "orgy", "ouch", 
                "oust", "ouzo", "oval", "oven", "over", "ovum", "owly", "oxen", "pace", "pack", "pact", "page", "paid", "pail", "pain", 
                "pair", "pale", "pall", "palm", "palp", "pane", "pang", "pant", "papa", "pare", "park", "part", "pass", "past", "pate", 
                "path", "pave", "pawn", "peak", "peal", "pear", "peat", "peck", "peed", "peek", "peel", "peep", "peer", "pelt", "pend", 
                "pent", "peon", "perk", "pert", "pest", "phon", "pica", "pick", "pier", "pike", "pile", "pill", "pimp", "pine", "ping", 
                "pink", "pint", "pion", "pipe", "piss", "pith", "pity", "pixy", "plan", "plat", "play", "plea", "plod", "plop", "plot", 
                "plow", "ploy", "plug", "plum", "plus", "poem", "poet", "pogo", "poke", "pole", "poll", "polo", "pomp", "pond", "pong", 
                "pont", "pony", "pooh", "pool", "poop", "poor", "pope", "pore", "pork", "port", "pose", "posh", "post", "posy", "pour", 
                "pout", "pram", "pray", "prep", "prey", "prig", "prim", "prod", "prom", "prop", "prow", "puck", "puff", "puke", "pull", 
                "pulp", "puma", "pump", "punk", "punt", "puny", "pupa", "pure", "purl", "purr", "push", "putt", "pyre", "quad", "quay", 
                "quid", "quip", "quit", "quiz", "quod", "race", "rack", "racy", "raft", "rage", "raid", "rail", "rain", "rake", "ramp", 
                "rang", "rank", "rant", "rape", "rapt", "rare", "rasa", "rash", "rasp", "rata", "rate", "rave", "raze", "read", "real", 
                "ream", "reap", "rear", "reck", "reed", "reef", "reek", "reel", "rein", "rend", "rent", "rest", "rice", "rich", "rick", 
                "ride", "rife", "rift", "rill", "rime", "rimy", "ring", "rink", "riot", "ripe", "rise", "risk", "rite", "road", "roam", 
                "roar", "robe", "rock", "rode", "roil", "role", "roll", "romp", "rood", "roof", "rook", "room", "root", "rope", "ropy", 
                "rose", "rosy", "rote", "rout", "rove", "ruby", "rude", "ruff", "ruin", "rule", "rump", "rune", "rung", "runt", "ruse", 
                "rush", "rusk", "rust", "sack", "safe", "saga", "sage", "sago", "said", "sail", "sake", "sale", "salt", "same", "sand", 
                "sane", "sang", "sank", "sans", "sari", "sash", "save", "scab", "scad", "scam", "scan", "scar", "scat", "scud", "scum", 
                "seal", "seam", "sear", "seat", "sect", "seed", "seek", "seem", "seen", "seep", "self", "sell", "semi", "send", "sent", 
                "sept", "sera", "serf", "sewn", "sexy", "shad", "shag", "shah", "sham", "shaw", "shay", "shed", "shim", "shin", "ship", 
                "shiv", "shod", "shoe", "shoo", "shop", "shot", "show", "shun", "shut", "sial", "sick", "side", "sift", "sigh", "sign", 
                "silk", "sill", "silo", "silt", "sima", "sine", "sing", "sinh", "sink", "sire", "site", "situ", "siva", "size", "skat", 
                "skew", "skid", "skim", "skin", "skip", "skit", "slab", "slag", "slam", "slap", "slat", "slay", "sled", "slew", "slid", 
                "slim", "slip", "slit", "slob", "sloe", "slog", "slop", "slot", "slow", "slug", "slum", "slur", "slut", "smog", "smug", 
                "smut", "snag", "snap", "snip", "snob", "snow", "snub", "snug", "soak", "soap", "soar", "sock", "soda", "sofa", "soft", 
                "soil", "sold", "sole", "solo", "soma", "some", "song", "soon", "soot", "sora", "sorb", "sore", "sort", "soul", "soup", 
                "sour", "sown", "soya", "span", "spar", "spat", "spay", "spec", "sped", "spew", "spin", "spit", "spot", "spud", "spun", 
                "spur", "stab", "stag", "star", "stay", "stem", "step", "stew", "stir", "stop", "stow", "stub", "stud", "stun", "such", 
                "suck", "suds", "suet", "suey", "suit", "sulk", "sung", "sunk", "sure", "surf", "swab", "swag", "swam", "swan", "swap", 
                "swat", "sway", "swig", "swim", "swum", "tabu", "tack", "tact", "taft", "tail", "take", "talc", "tale", "talk", "tall", 
                "tame", "tamp", "tang", "tanh", "tank", "tapa", "tape", "tara", "taro", "tart", "task", "tate", "taut", "taxa", "taxi", 
                "teak", "teal", "team", "tear", "teat", "tech", "teem", "teen", "teet", "tell", "tend", "tent", "term", "tern", "test", 
                "tete", "text", "than", "that", "thaw", "thee", "them", "then", "they", "thin", "this", "thou", "thud", "thug", "thus", 
                "tick", "tide", "tidy", "tied", "tier", "tift", "tile", "till", "tilt", "time", "tine", "tint", "tiny", "tire", "toad", 
                "tofu", "togs", "toil", "told", "toll", "tomb", "tome", "tone", "tong", "tonk", "tony", "took", "tool", "toot", "tore", 
                "tori", "torn", "torr", "tort", "tory", "toss", "tote", "toto", "tour", "tout", "town", "trag", "tram", "trap", "tray", 
                "tree", "trek", "trig", "trim", "trio", "trip", "trod", "trot", "troy", "true", "tsar", "tset", "tuba", "tube", "tuck", 
                "tuff", "tuft", "tuna", "tune", "tung", "turf", "turk", "turn", "tusk", "tutu", "twig", "twin", "twit", "tyke", "type", 
                "typo", "ugly", "ulna", "unit", "upon", "urea", "urge", "vade", "vain", "vale", "vamp", "vane", "vary", "vase", "vast", 
                "veal", "veer", "veil", "vein", "vend", "vent", "verb", "very", "vest", "veto", "vial", "vice", "vide", "view", "viii", 
                "vile", "vine", "visa", "vise", "vita", "viva", "vivo", "void", "vole", "volt", "vote", "wack", "wade", "wadi", "wage", 
                "wail", "wait", "wake", "wale", "walk", "wall", "wand", "wane", "want", "ward", "ware", "warm", "warn", "warp", "wart", 
                "wary", "wash", "wasp", "wast", "watt", "wave", "wavy", "waxy", "weak", "weal", "wean", "wear", "weed", "week", "weep", 
                "weir", "weld", "well", "welt", "went", "wept", "were", "wert", "west", "wham", "what", "whee", "when", "whet", "whey", 
                "whig", "whim", "whip", "whir", "whit", "whiz", "whoa", "whom", "whop", "whup", "wick", "wide", "wife", "wild", "wile", 
                "will", "wilt", "wily", "wind", "wine", "wing", "wink", "wino", "winy", "wipe", "wire", "wiry", "wise", "wish", "wisp", 
                "with", "wive", "woke", "wold", "wolf", "womb", "wont", "wood", "wool", "word", "wore", "work", "worm", "worn", "wove", 
                "wrap", "wren", "writ", "wynn", "yang", "yank", "yard", "yarn", "yawl", "yawn", "yeah", "year", "yell", "yelp", "yoga", 
                "yogi", "yoke", "yolk", "yond", "yore", "your", "yuck", "yule", "zeal", "zero", "zest", "zeta", "zinc", "zing", "zone", 
                "zoom" 
            };
        }
    }
}