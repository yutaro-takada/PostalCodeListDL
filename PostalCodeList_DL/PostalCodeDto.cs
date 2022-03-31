namespace SeleniumTest
{
    class PostalCodeDto
    {
        #region 定数  
        private const int COLUMN_COUNT_DATA_RECORD = 15;

        #endregion

        #region プロパティ 
        //[0]全国地方公共団体コード
        public string zenkokuCode { get; set; }

        //[1](旧)郵便番号
        public string oldPostalCode { get; set; }

        //[2]郵便番号
        public string postalCode { get; set; }

        //[3]都道府県名カナ
        public string prefecturesNameKana { get; set; }

        //[4]市区町村名カナ
        public string municipalitiesNameKana { get; set; }

        //[5] 町域名カナ
        public string townAreaNameKana { get; set; }

        //[6]都道府県名
        public string prefecturesName { get; set; }

        //[7]市区町村名
        public string municipalitiesName { get; set; }

        //[8]町域名
        public string townAreaName { get; set; }

        //[9]郵便番号が2つ以上ある町域(該当:1)
        public string twinPostalCodeArea { get; set; }

        //[10]小字毎に番地が設定されている町域(該当:1)
        public string koazaBanchi { get; set; }

        //[11]丁目を有する町域(該当:1)
        public string hasTyoumeArea { get; set; }

        //[12]郵便番号が重複する町域(該当:1)
        public string onePostalCodeTwoArea { get; set; }

        //[13]更新の表示
        public string updateRecord { get; set; }

        //[14]変更理由
        public string updateReason { get; set; }

        #endregion

        #region パブリックメソッド
        public PostalCodeDto SetCsv(string dataRecord)
        {
            string[] datas = dataRecord.Split(',');

            var dto = new PostalCodeDto();

            dto.zenkokuCode = datas[0];
            dto.oldPostalCode = datas[1];
            dto.postalCode = datas[2];
            dto.prefecturesNameKana = datas[3];
            dto.municipalitiesNameKana = datas[4];
            dto.townAreaNameKana = datas[5];
            dto.prefecturesName = datas[6];
            dto.municipalitiesName = datas[7];
            dto.townAreaName = datas[8];
            dto.twinPostalCodeArea = datas[9];
            dto.koazaBanchi = datas[10];
            dto.hasTyoumeArea = datas[11];
            dto.onePostalCodeTwoArea = datas[12];
            dto.updateRecord = datas[13];
            dto.updateRecord = datas[14];

            return dto;
        }

        #endregion

    }
}