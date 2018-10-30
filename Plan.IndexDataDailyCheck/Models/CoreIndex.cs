namespace Plan.IndexDataDailyCheck.Models
{
    public class CoreIndex
    {
        /// <summary>
        /// 指标编码
        /// </summary>
        public string IndexCode { get; set; }

        /// <summary>
        /// 指标名称
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// 指标来源
        /// </summary>
        public int IndexSourceType { get; set; }

        /// <summary>
        /// 指标维护人
        /// </summary>
        public string AdminName { get; set; }
    }
}