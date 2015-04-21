using System;
using System.Management;

namespace MyWpfFramework.Common.Logger
{
    /// <summary>
    /// دریافت اطلاعات پردازنده سیستم جهت ثبت در فایل وقایع برنامه
    /// </summary>
    public class ProcessorInformation
    {

        /// <summary>
        /// Get the Caption of the Cpu (some rando-memum of the Cpu)
        /// </summary>        
        public static string GetCpuCaption()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["Caption"].ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }


        /// <summary>
        /// Get the current operating voltage of the Cpu
        /// </summary>        
        public static int GetCpuVoltage()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return Convert.ToInt32(queryObj["CurrentVoltage"]);
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// Dual core? Quad-core? "Many-core"?  http://en.wikipedia.org/wiki/Many-core_processing_unit
        /// </summary>        
        public static int GetCpuCores()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return Convert.ToInt32(queryObj["NumberOfCores"]);
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// Cpu serial
        /// </summary>        
        public static string GetCpuId()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["ProcessorId"].ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Cpu socket designation, more info here: http://en.wikipedia.org/wiki/CPU_socket
        /// </summary>        
        public static string GetCpuSocketDesignation()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["SocketDesignation"].ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Intel vs AMD :)
        /// </summary>        
        public static string GetCpuManufacturer()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["Manufacturer"].ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// think hyper-threading
        /// </summary>        
        public static int GetCpuNumberOfLogicalProcessors()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return Convert.ToInt32(queryObj["NumberOfLogicalProcessors"]);
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// Gets the CPU's Clock Speed
        /// </summary>        
        public static int GetCpuClockSpeed()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return Convert.ToInt32(queryObj["CurrentClockSpeed"]);
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }

        /// <summary>
        /// Reads whether you are running a 32 or 64 Bit system
        /// </summary>        
        public static int GetCpuDataWidth()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return Convert.ToInt32(queryObj["DataWidth"]);
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }
    }
}