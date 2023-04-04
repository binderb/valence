using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtomDictionary : MonoBehaviour {

	public Dictionary<string,string> moleculeDictionary = new Dictionary<string,string>();
	public Dictionary<string,string> codexDictionary = new Dictionary<string,string>();
	public List<string> lightsDictionary = new List<string>();
	public List<string> clueDictionary = new List<string>();

	// Use this for initialization
	void Start () {



		/* ----------------------------------------------
		 * Build Molecule dictionary
		 * ----------------------------------------------*/

		//Diatoms
		moleculeDictionary.Add ("HH1,HH1","Molecular Hydrogen");
		moleculeDictionary.Add ("OO2,OO2","Molecular Oxygen");
		moleculeDictionary.Add ("NN3,NN3","Molecular Nitrogen");

		//Nonmetal + Hydrogen
		moleculeDictionary.Add ("HO1,HO1,OH1OH1","Water");

		//Oxides
		moleculeDictionary.Add ("CO2CO2,OC2,OC2","Carbon Dioxide");
		moleculeDictionary.Add ("CC2CO2,CC2CO2,OC2,OC2","Ethylene Dione");
		moleculeDictionary.Add ("CC2CC2,CC2CO2,CC2CO2,OC2,OC2","Carbon Suboxide");
		moleculeDictionary.Add ("HO1,NO1NO2,OH1ON1,ON2","Nitrous Acid");
		moleculeDictionary.Add ("HO1,HO1,OH1OO1,OH1OO1","Hydrogen Peroxide");
		moleculeDictionary.Add ("HN1,HN1,HN1,HN1,NH1NH1NO1,NH1NH1NO1,ON1ON1","Diazoxane");
		moleculeDictionary.Add ("CH1CH1CH1CO1,CH1CH1CH1CO1,HC1,HC1,HC1,HC1,HC1,HC1,OC1OO1,OC1OO1","Methyl Peroxide");
		moleculeDictionary.Add ("HN1,HN1,HN1,HO1,NH1NH1NO1,NH1NO1NO1,OH1ON1,ON1ON1","Unstable Compound!");

		//Alkanes
		moleculeDictionary.Add ("CH1CH1CH1CH1,HC1,HC1,HC1,HC1","Methane");
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CH1CH1CH1,HC1,HC1,HC1,HC1,HC1,HC1","Ethane");
		moleculeDictionary.Add ("CC1CC1CH1CH1,CC1CH1CH1CH1,CC1CH1CH1CH1,HC1,HC1,HC1,HC1,HC1,HC1,HC1,HC1","Propane");

		//Alkenes
		moleculeDictionary.Add ("CC2CH1CH1,CC2CH1CH1,HC1,HC1,HC1,HC1","Ethene");
		moleculeDictionary.Add ("CC2CH1CH1,CC2CC1CH1,CC1CH1CH1CH1,HC1,HC1,HC1,HC1,HC1","Propene");

		//Alkynes
		moleculeDictionary.Add ("CC3CH1,CC3CH1,HC1,HC1","Acetylene");
		//i.e. Ethyne
		moleculeDictionary.Add ("CC1CC3,CC1CH1CH1CH1,CC3CH1,HC1,HC1,HC1,HC1","Propyne");

		//Dienes
		//moleculeDictionary.Add ("CC2CC2,CC2CH1CH1,CC2CH1CH1,HC1,HC1,HC1,HC1","Allene"); //i.e. Propadiene
		moleculeDictionary.Add ("CC2CC2,CC2CH1CH1,CC2CH1CH1,HC1,HC1,HC1,HC1","Allene");

		//Alcohols
		moleculeDictionary.Add ("CH1CH1CH1CO1,HC1,HC1,HC1,HO1,OC1OH1","Methanol");
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CH1CH1CO1,HC1,HC1,HC1,HC1,HC1,HO1,OC1OH1","Ethanol");
		moleculeDictionary.Add ("CC1CC1CH1CH1,CC1CH1CH1CH1,CC1CH1CH1CO1,HC1,HC1,HC1,HC1,HC1,HC1,HC1,HO1,OC1OH1","Propanol");
		moleculeDictionary.Add ("CC1CC1CH1CO1,CC1CH1CH1CH1,CC1CH1CH1CH1,HC1,HC1,HC1,HC1,HC1,HC1,HC1,HO1,OC1OH1","Isopropanol");
		moleculeDictionary.Add ("CH1CH1CO1CO1,HC1,HC1,HO1,HO1,OC1OH1,OC1OH1","Methylene Glycol");
		moleculeDictionary.Add ("CC2CH1CO1,CC2CH1CO1,HC1,HC1,HO1,HO1,OC1OH1,OC1OH1","Ethenediol");
		//i.e. Methanediol
		moleculeDictionary.Add ("CC1CH1CH1CO1,CC1CH1CH1CO1,HC1,HC1,HC1,HC1,HO1,HO1,OC1OH1,OC1OH1","Ethylene Glycol");
		//i.e. Ethanediol

		//Ethers
		moleculeDictionary.Add ("CH1CH1CH1CO1,CH1CH1CN1CO1,HC1,HC1,HC1,HC1,HC1,HN1,HN1,NC1NH1NH1,OC1OC1","Methoxymethylamine");


		//Carboxylic Acids
		moleculeDictionary.Add ("CH1CO1CO2,HC1,HO1,OC1OH1,OC2","Formic Acid");
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CO1CO2,HC1,HC1,HC1,HO1,OC1OH1,OC2","Acetic Acid");
		moleculeDictionary.Add ("CC1CC1CH1CH1,CC1CH1CH1CH1,CH1CO1CO2,HC1,HC1,HC1,HC1,HC1,HO1,OC1OH1,OC2","Propionic Acid");
		moleculeDictionary.Add ("CC1CH1CH1CN1,CC1CO1CO2,HC1,HC1,HN1,HN1,HO1,NC1NH1NH1,OC1OH1,OC2","Glycine");
		moleculeDictionary.Add ("CC1CC2CH1,CC1CO1CO2,CC2CH1CH1,HC1,HC1,HC1,HO1,OC1OH1,OC2","Acrylic Acid");
		moleculeDictionary.Add ("CO1CO1CO2,HO1,HO1,OC1OH1,OC1OH1,OC2","Carbonic Acid");

		//Esters
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CO1CO2,CH1CH1CH1CO1,HC1,HC1,HC1,HC1,HC1,HC1,OC1OC1,OC2","Methyl Acetate");

		//Aldehydes
		moleculeDictionary.Add ("CH1CH1CO2,HC1,HC1,OC2","Formaldehyde");
		//i.e. Methanal
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CH1CO2,HC1,HC1,HC1,HC1,OC2","Acetaldehyde");
		//i.e. Ethanal
		moleculeDictionary.Add ("CC1CH1CO2,CC1CH1CO2,HC1,HC1,OC2,OC2","Glyoxal");
		//i.e. Oxalaldehyde
		moleculeDictionary.Add ("CC1CH1CH1CO1,CC1CH1CO2,HC1,HC1,HC1,HO1,OC1OH1,OC2", "Glycolaldehyde");
		moleculeDictionary.Add ("CC1CC3,CC1CH1CO2,CC3CH1,HC1,HC1,OC2", "Propynal");

		//Ketones
		moleculeDictionary.Add ("CC1CC1CO2,CC1CH1CH1CH1,CC1CH1CH1CH1,HC1,HC1,HC1,HC1,HC1,HC1,OC2","Acetone"); //i.e. Propanone

		//Ketenes
		moleculeDictionary.Add ("CC2CH1CH1,CC2CO2,HC1,HC1,OC2","Ethenone");
		moleculeDictionary.Add ("CC2CN2,CC2CO2,HN1,NC2NH1,OC2","Iminoethenone");
		//moleculeDictionary.Add ("CC2CO2,CC2CO2,OC2,OC2", "Oxoketene");

		//Azanes
		moleculeDictionary.Add ("HN1,HN1,HN1,NH1NH1NH1","Ammonia");
		moleculeDictionary.Add ("HN1,HN1,HN1,HN1,NH1NH1NN1,NH1NH1NN1","Hydrazine");
		moleculeDictionary.Add ("HN1,HN1,HN1,HO1,NH1NH1NN1,NH1NN1NO1,OH1ON1","Hydroxyhydrazine");

		//Azo Compounds (R-N=N-R)
		moleculeDictionary.Add ("HN1,HN1,NH1NN2,NH1NN2","Diazene");

		//Amines
		moleculeDictionary.Add ("CH1CH1CH1CN1,HC1,HC1,HC1,HN1,HN1,NC1NH1NH1","Methylamine");
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CH1CH1CN1,HC1,HC1,HC1,HC1,HC1,HN1,HN1,NC1NH1NH1","Ethylamine");
		moleculeDictionary.Add ("CH1CH1CN1CN1,HC1,HC1,HN1,HN1,HN1,HN1,NC1NH1NH1,NC1NH1NH1","Diaminomethane");
		moleculeDictionary.Add ("HN1,HN1,HO1,NH1NH1NO1,OH1ON1","Hydroxylamine");
		moleculeDictionary.Add ("CC3CH1,CC3CN1,HC1,HN1,HN1,NC1NH1NH1","Acetylenamine");

		//Imines (R2-C=N-R)
		moleculeDictionary.Add ("CH1CH1CN2,HC1,HC1,HN1,NC2NH1","Methanimine");
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CH1CN2,HC1,HC1,HC1,HC1,HN1,NC2NH1","Ethanimine");
		moleculeDictionary.Add ("CC2CH1CH1,CC2CN2,HC1,HC1,HN1,NC2NH1","Ethenimine");
		moleculeDictionary.Add ("CC3CN1,CC3CN1,HN1,HN1,HN1,HN1,NC1NH1NH1,NC1NH1NH1","Ethynediamine");

		moleculeDictionary.Add ("CH1CH1CN2,HC1,HC1,HN1,HN1,NC2NN1,NH1NH1NN1","Formaldehyde Hydrazone");

		moleculeDictionary.Add ("CH1CH1CH1CN1,CH1CH1CN2,HC1,HC1,HC1,HC1,HC1,NC1NC2","N-Methylmethanimine");

		//Nitriles (R-C≡N)
		moleculeDictionary.Add ("CH1CN3,HC1,NC3","Hydrogen Cyanide");
		moleculeDictionary.Add ("CN3CO1,HO1,NC3,OC1OH1","Cyanic Acid");
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CN3,HC1,HC1,HC1,NC3","Acetonitrile");

		//Nitroso Compounds (R-N=O)
		moleculeDictionary.Add ("HN1,NH1NO2,ON2","Azanone");
		moleculeDictionary.Add ("CH1CH1CH1CN1,HC1,HC1,HC1,NC1NO2,ON2","Nitrosomethane");
		moleculeDictionary.Add ("CC1CH1CH1CH1,CC1CH1CH1CN1,HC1,HC1,HC1,HC1,HC1,NC1NO2,ON2","Nitrosoethane");
		moleculeDictionary.Add ("CC2CH1CH1,CC2CH1CN1,HC1,HC1,HC1,NC1NO2,ON2","Nitrosoethene");
		moleculeDictionary.Add ("HN1,HO1,NH1NN2,NN2NO1,OH1ON1","Nitrosamine");
		moleculeDictionary.Add ("HN1,HN1,NH1NH1NN1,NN1NO2,ON2"," Nitrosamine ");
		// (resonance with above)

		//Amides
		moleculeDictionary.Add ("CH1CN1CO2,HC1,HN1,HN1,NC1NH1NH1,OC2","Formamide");
		moleculeDictionary.Add ("CH1CN2CO1,HC1,HN1,HO1,NC2NH1,OC1OH1"," Formamide ");
		// (resonance with above)
		//moleculeDictionary.Add ("CH1CN1CO2,HC1,HN1,HN1,NC1NH1NH1,OC2"
		moleculeDictionary.Add ("CN2CO2,HN1,NC2NH1,OC2","Methenamide");
		moleculeDictionary.Add ("CN1CO1CO2,HN1,HN1,HO1,NC1NH1NH1,OC1OH1,OC2","Carbamic Acid");
		moleculeDictionary.Add ("CN1CN3,HN1,HN1,NC1NH1NH1,NC3","Cyanamide");
		moleculeDictionary.Add ("CN1CN1CO2,HN1,HN1,HN1,HN1,NC1NH1NH1,NC1NH1NH1,OC2", "Urea");
		moleculeDictionary.Add ("CC1CC2CH1,CC1CN1CO2,CC2CH1CH1,HC1,HC1,HC1,HN1,HN1,NC1NH1NH1,OC2", "Acrylamide");

		//Imides
		moleculeDictionary.Add ("CN2CN2,HN1,HN1,NC2NH1,NC2NH1", "Carbodiimide");


		/* ----------------------------------------------
		 * Build Codex dictionary
		 * ----------------------------------------------*/

		codexDictionary.Add ("Molecular Hydrogen","Hydrogen is the lightest, most common element in the universe, serving as the fuel for the brilliant nuclear reactions that occur in stars. Stars are actually too hot for gaseous molecules to form, so their hydrogen mostly exists in an ionized form called plasma.\n\nOn Earth, gaseous hydrogen has been considered as an alternative clean fuel source. If perfected, hydrogen fuel cells would be very clean indeed—the only 'waste' product of a fuel cell is water.");
		codexDictionary.Add ("Molecular Oxygen","Oxygen is fundamental for sustaining life on multiple levels: it is crucial in respiration, makes up 21% of our atmosphere, and enables the rapid release of heat via combustion chemistry. The release of oxygen gas by plants during photosynthesis also sets up a valuable chemical symbiosis between plants and animals.");
		codexDictionary.Add ("Molecular Nitrogen","Gaseous molecular nitrogen is the most abundant molecule in the atmosphere, making up roughly 78% of the air we breathe.\n\nContaining a strong nitrogen-nitrogen triple bond, molecular nitrogen is very unreactive, but elemental nitrogen makes up an essential portion of the nucleic acid and protein molecules that sustain life. A handful of tiny but essential microorganisms take on the responsibility of breaking molecular nitrogen down for the ecosystem.");
		codexDictionary.Add ("Water","Water is essential for all known life: the polar nature of its structure makes it an excellent solvent, and nearly all the chemical reactions that sustain life take place in aqueous solution.\n\nWater covers roughly 71% of the Earth's surface, and adult humans are 50-65%\nwater by mass.");
		codexDictionary.Add ("Hydrogen Peroxide","Dilute solutions of this reactive compound are commonly used as disinfectants in basic first aid.\n\nThe reactivity of hydrogen peroxide is due to its unstable oxygen-oxygen single bond (called a 'peroxide' bond). Acting as a powerful oxidizer, this molecule can readily disrupt and break down bacterial cell walls and disrupt cellular processes.\n\nSmall amounts are routinely used to disinfect cuts and whiten teeth.");
		codexDictionary.Add ("Formaldehyde","This molecule is incredibly important in the science fields for its ability to fix and preserve biological tissue; for this reason, it is also used to embalm the bodies of the deceased.\n\nFormaldehyde is usually used as a dilute solution in buffered saline, which is called 'formalin'. It preserves tissue structures by covalently binding nearby proteins together within cells, causing everything to become 'fixed' in place. Obviously, this ability makes it very toxic to living organisms!");
		codexDictionary.Add ("Formic Acid","This highly acidic molecule is used by certain species of ants as a defense against predators, embuing them with a notorious stinging bite.\n\nHumans synthesize large quantities of formic acid in the chemical industry, for use as a preservative and antibacterial agent.\n\nFormic acid is the simplest member of a family of organic acids known as the carboxylic acids. There is one more acid from this family in the Codex!");
		codexDictionary.Add ("Formamide","Formamide is mainly used as a building block in organic chemistry, but scientists have also been fascinated with its hypothetical role in extraterrestrial life.\n\nNo other planets currently discovered contain enough liquid water to support biological life as we know it... but there may be alternatives, and formamide is one of them. Formamide is an excellent solvent, and has similar electrostatic properties to water. Could there be formamide-based life out there? It's an open question!");
		codexDictionary.Add ("Acetylene","This reactive carbon compound is the hottest-burning common fuel gas in industry, making it especially useful for the welding and cutting of metal. The flame produced by burning acetylene in a combustion reaction with oxygen can reach 3,500 degrees Celsius.\n\nAcetylene is also the simplest in a family of compounds called 'alkynes', characterized by their carbon-carbon triple bonds. They are highly reactive and often used as basic building blocks in organic chemistry.");
		codexDictionary.Add ("Hydrogen Cyanide","Hydrogen cyanide has some useful applications as a building block in organic chemistry, but is best known as a deadly chemical weapon. It inhibits the body's ability to use oxygen in respiration, essentially causing suffocation even though the lungs continue to work properly.\n\nTrue toxicity comes from the cyanide ion formed when the hydrogen is removed; although many compounds can form this ion, hydrogen cyanide is particularly dangerous because it is a gas, and can be inhaled.");
		codexDictionary.Add ("Ammonia","All plants need a source of nitrogen to grow, and most can't use nitrogen gas in the air as a source; instead, they rely on precious ammonia in the soil.\n\nHelpful soil-dwelling bacteria convert nitrogen gas to ammonia in a process called 'nitrogen fixation'. Plants absorb this ammonia (and other downstream products), thus bringing nitrogen into the food chain.\n\nThe main use of ammonia is as a fertilizer, but it is also employed as a cleaning agent.");
		codexDictionary.Add ("Carbon Dioxide","This molecule plays vital roles in the cells of most organisms, being both a product of cellular respiration and an ingredient in the photosynthesis reaction.\n\nOf course,  carbon dioxide is also known as a 'greenhouse gas', because it tends to absorb and re-emit thermal radiation in the atmosphere that would otherwise escape into space. The burning of fossil fuels by humans has substantially increased the amount of atmospheric carbon dioxide since the industrial revolution.");
		codexDictionary.Add ("Methane","This molecule, along with a close relative called ethane, is an invaluable source of energy in the form of natural gas. Natural gas is burned to heat homes, cook food, and generate electricity—depending on the source, methane typically comprises 87-97% of this precious fuel.\n\nMethane is also the simplest in a family of compounds called alkanes, which are composed exclusively of hydrogen and carbon, and contain only single bonds.");
		codexDictionary.Add ("Ethane","This molecule, along with a close relative called methane, is an invaluable source of energy in the form of natural gas. Natural gas is burned to heat homes, cook food, and generate electricity—depending on the source, methane typically comprises 1.5-7% of this precious fuel.\n\nEthane is also used as a critical building block for the synthesis of other compounds. This process often starts by converting it first into ethene (another very similar compound, containing a double bond).");
		codexDictionary.Add ("Propane","Another alkane molecule especially familiar for its use as a fuel for outdoor grilling, propane is a heavier and less-volatile gas than ethane or methane. This makes propane easier to transport in individual containers, making it a good alternative to natural gas where municipal sources of the latter are not available.");
		codexDictionary.Add ("Ethene","Ethene (also called 'ethylene') plays an incredible role in the life cycle of plants—it induces ripening of fruit, shedding of leaves in the fall, and germination of seeds, among many other things.\n\nIt turns out that this molecule is the reason why you can make fruit ripen faster if you seal several pieces together in a paper or plastic bag. Ripening fruit gives off ethene gas as it matures, and in a closed space, that ethene can build up and accelerate the ripening process further!");
		codexDictionary.Add ("Methanol","Methanol is the simplest in a family of flammable compounds called alcohols, and it happens to be the primary fuel used in the engines of monster trucks and drag racers. This is because methanol burns without opaque smoke or a visible flame, making it a safer alternative in high-risk automotive sports where clouds of smoke would be especially hazardous.\n\nMethanol is also a favorite fuel source for portable camping stoves, because it burns well in a simple, open container.");
		codexDictionary.Add ("Ethanol","This compound is probably most well known for its use in alcoholic beverages—ethanol is the specific chemical name for the 'alcohol' found in beers, wines, and spirits.\n\nIn reality, alcohols are a whole family of compounds, distinguished by their hydroxyl (OH) groups and carbon-containing backbones.\n\nEthanol has many other important uses—it is employed extensively as a biofuel, solvent, and sterilizing agent.");
		codexDictionary.Add ("Isopropanol","Isopropanol is known more commonly by the name 'rubbing alcohol', and is used extensively as the main component in many domestic cleaners, solvents, and hand sanitizers.\n\nSolutions of isopropanol (like most alcohols) are toxic to bacteria and can thus be used as sterilizing agents. Compared to other compounds with similar properties, however, the toxicity of isopropanol to humans is low, which explains how popular it's become for topical use.");
		codexDictionary.Add ("Methylamine","This compound plays a crucial role in putrefaction (the breakdown of organic matter), and is partly responsible for the smell of rotten fish. Members of the amine family have vast importance for the flow of nutrients in the biosphere, but many of them have similarly unpleasant odors.");
		codexDictionary.Add ("Acetaldehyde","Acetaldehyde has a rough reputation—it's the first by-product of the body's effort to process ethanol; evidence links its elevated presence to the feelings of misery associated with a hangover.\n\nAcetaldehyde can also be present as an impurity in the brewing and wine-making processes, lending an odd green- or rotten-apple taste to beverages that have not been fermented for long enough.");
		codexDictionary.Add ("Acetic Acid","Acetic acid is the compound that gives vinegar its distinctive smell and properties; vinegar is basically a solution of roughly 10% acetic acid.\n\nIn biological systems, this entire molecule functions as a 'tag' that can be tacked onto large biomolecules like proteins, to provide a chemical signal. This process is called 'acetylation', and is especially important for the histone proteins that organize DNA in the cell's nucleus.");
		codexDictionary.Add ("Acetone","Acetone is a great solvent for many materials (like plastics and adhesives) that wouldn't normally dissolve; for this reason, its main domestic use is as the primary component in nail polish remover.\n\nAcetone's dissolving power is tied directly to its structure—it's two methyl groups (CH₃) lend it a very nonpolar character on one end, and its carbonyl (CO) group is very polar, by contrast. These characteristics allow it to dissolve both hydrophilic and hydrophobic compounds.");
		codexDictionary.Add ("Ethylene Glycol","This compound has a unique aptitude for disrupting the hydrogen bond network between water molecules that would normally form in ice, making it a useful and effective antifreeze.\n\nEthylene Glycol can also be used in many manufacturing applications as a precursor to plastic polymers. Plastics are prepared by reacting large numbers of molecules so that they form long repeating chains, and ethylene glycol can be reacted in this way to produce the plastic found in soda bottles.");
		codexDictionary.Add ("Methyl Acetate","Methyl acetate belongs to a family of compounds called esters, which are collectively responsible for the sweet, pleasing scents of many flowers, fruits, and berries.\n\nMethyl acetate itself is a bit boring, having the pungent smell we usually associate with glue (because it is often used as an ingredient in adhesives). Longer and more complex carbon chains surrounding the O-C=O motif yield more interesting aromas.");
		codexDictionary.Add ("Hydrazine","Hydrazine has a number of important applications in industrial processes: chiefly, it is used as a 'blowing agent' for the production of polymer products with airy textures, like styrofoam.\n\nHydrazine is also used in the preparation of chemicals in air bags. When a seal is broken by impact and the chemicals are allowed to mix, a large amount of nitrogen gas is produced, which inflates the bag.");
		codexDictionary.Add ("Carbonic Acid","Carbonic acid can be formed after the dissolution of carbon dioxide in water; thus it plays a role in the biosphere for its contribution to ocean acidification. As more and more carbon dioxide is produced and absorbed by Earth's oceans, the formation of this acid has had subtle effects on the pH of ocean water, which can have a dire impact on shellfish especially.\n\nCarbonic acid is also involved in blood chemistry, where it provides a means to shuttle CO₂ out of the body.");
		codexDictionary.Add ("Acrylic Acid","If you're an artist, this compound's name might be familiar: acrylic acid is the major building block for the polymer emulsion we commonly know as acrylic paint.\n\nAcrylic acid is a good example of a compound that readily reacts with itself to form long chains of repeating subunits—this process is called 'polymerization', and is encountered very often in biochemistry and material science.");
		codexDictionary.Add ("Acrylamide","Acrylamide is a moderate neurotoxin in its isolated form, but when it reacts with itself to form polyacrylamide, it becomes a valuable tool for biochemists. For decades, scientists have used gels made of this material as a matrix for separating the individual proteins in cell lysates and other biological preparations.\n\nIndividual units (monomers) combining into long repeating chains (polymers) is a common theme in biochemistry and material science—find more examples on this list!");
		codexDictionary.Add ("Glycine","Congratulations! You've just made an amino acid, one of the basic building blocks for a huge family of biomolecules called proteins. There are 20 common amino acids used by the human body, and they all have the same basic structure: an amino group (H-N-H) on one end, and a carboxyl group (O=C-O-H) on the other.\n\nDifferent food items contain differing amounts of each amino acid, with gelatin being particularly rich in glycine.");

		// Urea
		// Dimethylamine (pheramone for insects)
		// Diethyl Ether
		// Glycolaldehyde
		// Cyanogen (N=-C-C-=N) (first cyanide isolated from Prussian blue pigment, soaps for photographers)



		/* ----------------------------------------------
		 * Build Lights dictionary
		 * ----------------------------------------------*/

		lightsDictionary.Add ("Molecular Hydrogen");
		lightsDictionary.Add ("Molecular Oxygen");
	  lightsDictionary.Add ("Molecular Nitrogen");
		lightsDictionary.Add ("Water");
		lightsDictionary.Add ("Hydrogen Peroxide");
		lightsDictionary.Add ("Formaldehyde");
		lightsDictionary.Add ("Formic Acid");
		lightsDictionary.Add ("Formamide");
		lightsDictionary.Add ("Acetylene");
		lightsDictionary.Add ("Hydrogen Cyanide");
		lightsDictionary.Add ("Ammonia");
		lightsDictionary.Add ("Carbon Dioxide");
		lightsDictionary.Add ("Methane");
		lightsDictionary.Add ("Ethane");
		lightsDictionary.Add ("Propane");
		lightsDictionary.Add ("Ethene");
		lightsDictionary.Add ("Methanol");
		lightsDictionary.Add ("Ethanol");
		lightsDictionary.Add ("Isopropanol");
		lightsDictionary.Add ("Methylamine");
		lightsDictionary.Add ("Acetaldehyde");
		lightsDictionary.Add ("Acetic Acid");
		lightsDictionary.Add ("Acetone");
		lightsDictionary.Add ("Ethylene Glycol");
		lightsDictionary.Add ("Methyl Acetate");
		lightsDictionary.Add ("Hydrazine");
		lightsDictionary.Add ("Carbonic Acid");
		lightsDictionary.Add ("Acrylic Acid");
		lightsDictionary.Add ("Acrylamide");
		lightsDictionary.Add ("Glycine");


		/* ----------------------------------------------
		 * Build Clues dictionary
		 * ----------------------------------------------*/

		 clueDictionary.Add ("This molecule is one of the diatomic gases.");
		 clueDictionary.Add ("This molecule is another of the diatomic gases.");
		 clueDictionary.Add ("This molecule is yet another of the diatomic gases.");
		 clueDictionary.Add ("This well-known substance covers roughly 71% of the Earth's surface.");
		 clueDictionary.Add ("This simple compound has a peroxide bond, which is a single bond between two oxygen atoms.");
		 clueDictionary.Add ("This is the simplest possible aldehyde. Aldehydes have a group that looks like this:\nO=C-H");
		 clueDictionary.Add ("This is the simplest possible carboxylic acid. Carboxylic acids have a group that looks like this:\nO=C-O-H");
		 clueDictionary.Add ("This is the simplest possible amide. Amides have a group that looks like this:\nO=C-NH₂");
		 clueDictionary.Add ("This simple compound is an alkyne, a molecule containing a high-order carbon-carbon bond.");
		 clueDictionary.Add ("This simple compound forms a high-order bond between two different elements.");
		 clueDictionary.Add ("What happens when you saturate a common group VA element with hydrogen?");
		 clueDictionary.Add ("This compound has only three atoms, and two are oxygen.");
		 clueDictionary.Add ("What happens when you saturate a common group IVA element with hydrogen?");
		 clueDictionary.Add ("What can you make if you only use carbon and hydrogen with single bonds?");
		 clueDictionary.Add ("Keep thinking—are there other compounds of singly-bonded carbon and hydrogen you haven't made yet?");
		 clueDictionary.Add ("This simple hydrocarbon has one double bond, between two of the same element.");
		 clueDictionary.Add ("This is the simplest possible alcohol. Alcohols have an O-H group attached to carbon.");
		 clueDictionary.Add ("This is an alcohol with two carbons. Alcohols have an O-H group attached to carbon.");
		 clueDictionary.Add ("This is an alcohol with three carbons. Alcohols have an O-H group attached to carbon.");
		 clueDictionary.Add ("This is the simplest possible amine. Amines have an H-N-H group attached to carbon.");
		 clueDictionary.Add ("This compound is an aldehyde. Aldehydes have a group that looks like this:\nO=C-H");
		 clueDictionary.Add ("This compound is a carboxylic acid. Carboxylic acids have a group that looks like this:\nO=C-O-H");
		 clueDictionary.Add ("This is the simplest possible ketone. Ketones have a C=O group attached to two carbons.");
		 clueDictionary.Add ("This compound has 4 non-hydrogen atoms: two carbon, two oxygen.");
		 clueDictionary.Add ("Discover acetic acid (it's also in the Codex). What would happen if you replaced the O-H with O-C?");
		 clueDictionary.Add ("This compound contains only nitrogen and hydrogen.");
		 clueDictionary.Add ("Discover acetic acid (it's also in the Codex). What would happen if you replaced the CH₃ with O-H?");
		 clueDictionary.Add ("Formic acid and ethene are two other compounds in the Codex. Can you think of a creative way to combine their structures?");
		 clueDictionary.Add ("Formamide and ethene are two other compounds in the Codex. Can you think of a creative way to combine their structures?");
		 clueDictionary.Add ("This compound is an amino acid. Amino acids have both a carboxyl group (O=C-O-H) and an amino group (H-N-H) bonded to carbon.");








	}

	// Update is called once per frame
	void Update () {

	}
}
