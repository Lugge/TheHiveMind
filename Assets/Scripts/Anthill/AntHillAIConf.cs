using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This class holds the configuration of the anthill AI
 * 
 * @author: Lukas Krose
 * @version: 1.0
 */
namespace AntHill
{
	public class AntHillAIConf
	{
		public readonly int[] antRatio = new int[]{5,1};
		public readonly int maxThinkTime = 10;
		public readonly int randomness = 2;

		public readonly string foodToCollectFormula = "metric - searcherPerFood";
		public readonly Dictionary<string, double> initialEvalConfig = new Dictionary<string, double> ();
		public readonly Dictionary<string, Dictionary <string, string>> baseEvalFunc = new Dictionary<string, Dictionary <string, string>> ();
		public readonly Dictionary<string, Dictionary <string, string>> impact = new Dictionary<string, Dictionary <string, string>> ();
		/*
		public readonly Dictionary<string, double> initialEvalConfig = new Dictionary<string, double>(){
			{"nothing", 40},
			{"spawnSearcher", 0},
			{"spawnWorker", 0},
			{"sendSearcherToSearch", 0},
			{"sendSearcherToOptimize", 0},
			{"sendWorkerToCollect", 0},
		};
		public readonly Dictionary<string, Dictionary <string, string>> baseEvalFunc = new Dictionary<string, Dictionary <string, string>>(){
			{ "default", new Dictionary<string, string> {  
					{"nothing", "0"},
					{"spawnSearcher", "0"},
					{"spawnWorker", "0"},
					{"sendSearcherToSearch", "0"},
					{"sendWorkerToCollect", "0"},
					{"sendSearcherToOptimize", "-1000"},} }, 
			{ "foodCount", new Dictionary<string, string> {  
					{"spawnSearcher", "100- ( 100 / ( 1 +  ( ( foodCount - ( 100 * impact ) ) / 100)  ) )"},
					{"spawnWorker", "100- ( 100 / ( 1 +  ( ( foodCount - ( 100 * impact ) ) / 100)  ) )"},
					{"sendSearcherToSearch", "100- ( 100 / ( 1 +  ( ( foodCount - ( 100 * impact ) ) / 100)  ) )"},
					{"sendWorkerToCollect", "100- ( 100 / ( 1 +  ( ( foodCount - ( 100 * impact ) ) / 100)  ) )"},
					{"sendSearcherToOptimize", "-1000"},} }, 
			{ "foundFood", new Dictionary<string, string> {
					{"spawnSearcher", "100 / (1 + ( knownFood * impact ) )"},
					{"spawnWorker", "100- ( 100 / ( 1 +  ( ( knownFood * impact) ) ) )"},
					{"sendSearcherToSearch", "100 / ( 1 + ( knownFood * impact ) )"},
					{"sendWorkerToCollect", "100- ( 100 / ( 1 +  ( ( knownFood * impact) ) ) )"},
					{"sendSearcherToOptimize", "-1000"},} }, 
			{ "idleSearcherInBase", new Dictionary<string, string> {
					{"sendSearcherToSearch", "100 - ( 100 / (1 + ( searcherInBase * impact ) ) )"},
					{"sendSearcherToOptimize", "-1000"},} }, 
			{ "idleWorkerInBase", new Dictionary<string, string> { 
					{"sendWorkerToCollect", "100 - ( 100 / (1 + ( workerInBase * impact ) ) )"}, } }, 
			{ "searcherCount", new Dictionary<string, string> { 
					{"spawnSearcher", "100 / ( 1 + ( ( searcherAnts * impact ) / 3 ) )"},} }, 
			{ "workerCount", new Dictionary<string, string> {
					{"spawnWorker", "100 / ( 1 + ( ( workerAnts * impact ) / 3 ) )"}, } },
			
		};
		public readonly Dictionary<string, Dictionary <string, string>> impact = new Dictionary<string, Dictionary <string, string>>(){
			{ "default", new Dictionary<string, string> {} }, 
			{ "foodCount", new Dictionary<string, string> {  
					{"spawnSearcher", "3"},
					{"spawnWorker", "2"},
					{"sendSearcherToSearch", "2"},
					{"sendWorkerToCollect", "2"},
					{"sendSearcherToOptimize", "1"}, } },
			{ "foundFood", new Dictionary<string, string> {  
					{"spawnSearcher", "20"},
					{"spawnWorker", "20"},
					{"sendSearcherToSearch", "20"},
					{"sendSearcherToOptimize", "1"},
					{"sendWorkerToCollect", "20"}, } },
			{ "idleSearcherInBase", new Dictionary<string, string> { 
					{"sendSearcherToSearch", "100"},
					{"sendSearcherToOptimize", "1"},} }, 
			{ "idleWorkerInBase", new Dictionary<string, string> { 
					{"sendWorkerToCollect", "100"}, } }, 
			{ "searcherCount", new Dictionary<string, string> { 
					{"spawnSearcher", "1"},} }, 
			{ "workerCount", new Dictionary<string, string> { 
					{"spawnWorker", "1"},} }, 
			
		};
		 */


		/*
		 * Constructor. Sets the XML files and imports them.
		 * 
		 * @param: string initialConfgiXML XML for the initial configsettings
		 * @param: string baseEvalXML XML for the base eval functions
		 * @param: string impactXMl XML for the impact configuration
		 * @param: string configXML XML for all base configrations
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public AntHillAIConf (string initialConfigXML, string baseEvalXML, string impactXML, string configXML)
		{
			initialEvalConfig = XMLImport.importXML1DDouble (initialConfigXML);
			baseEvalFunc = XMLImport.importXML2D (baseEvalXML);
			impact = XMLImport.importXML2D (impactXML);
			//initialEvalConfig = XMLImport.importXML1D (initialConfigXML);
		}

	}
}

