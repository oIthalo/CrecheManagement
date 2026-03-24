namespace CrecheManagement.Domain.Responses.Dashboard;

public record DashboardResponse
{
    public string CrecheName { get; init; }
    public long TotalStudents { get; init; }
    public int TotalClassrooms { get; init; }
    public int PresentToday { get; init; }
    public int AbsentToday { get; init; }
    public double AttendanceRate { get; init; }
    public List<ClassroomDashboard> Classrooms { get; init; } = new();

    public record ClassroomDashboard
    {
        public string Name { get; init; }
        public int TotalStudents { get; init; }
        public int Present { get; init; }
        public int Absent { get; init; }
    }
}